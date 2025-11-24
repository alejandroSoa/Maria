using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Script para assets interactuables que hacen zoom cuando se les da click.
/// Requiere un BoxCollider2D para detectar clicks.
/// Añade este script a cada objeto con el que el jugador pueda interactuar.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class interactableAsset : MonoBehaviour
{
    [Header("Configuración de Zoom")]
    [Tooltip("Tamaño de la cámara cuando hace zoom (menor = más cerca)")]
    [SerializeField] private float zoomedSize = 2.5f;

    [Tooltip("Tamaño normal de la cámara (sin zoom)")]
    [SerializeField] private float normalSize = 5f;

    [Tooltip("Velocidad de la animación de zoom")]
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Referencias")]
    [Tooltip("Referencia a la cámara principal. Si se deja vacío, se busca automáticamente")]
    [SerializeField] private Camera mainCamera;

    [Tooltip("Canvas con los botones de navegación. Se oculta durante el zoom")]
    [SerializeField] private GameObject navigationCanvas;

    [Header("Interfaz del Item")]
    [Tooltip("Interfaz específica de este item que se activa en el segundo clic")]
    [SerializeField] private GameObject itemInterface;

    private static interactableAsset currentZoomedAsset = null;
    private static GameObject canvasReference = null;
    private bool isZoomed = false;
    private bool hasClickedOnce = false;
    private Vector3 originalCameraPosition;
    private Vector3 targetCameraPosition;

    private void Start()
    {
        // Configurar el Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
            rb.simulated = true;
        }

        // Configurar el BoxCollider2D
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = false;
        }

        // Buscar la cámara principal si no está asignada
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError($"InteractableAsset en {gameObject.name}: No se encontró la Main Camera.");
        }

        // Buscar el Canvas si no está asignado
        if (navigationCanvas == null)
        {
            navigationCanvas = GameObject.Find("Canvas");
        }

        // Guardar referencia estática al Canvas para que todos los assets la compartan
        if (navigationCanvas != null && canvasReference == null)
        {
            canvasReference = navigationCanvas;
        }

        Debug.Log($"[{gameObject.name}] InteractableAsset inicializado correctamente");
    }

    private void Update()
    {
        // Detectar clicks usando el nuevo Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Verificar si el mouse está sobre UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return; // Si está sobre UI, no hacer nada
            }

            // Obtener la posición del mouse en el mundo
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            // Hacer raycast para ver si tocó este objeto
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log($"¡CLICK DETECTADO en {gameObject.name}!");
                HandleClick();
            }
            else if (isZoomed && currentZoomedAsset == this)
            {
                // Si este objeto está con zoom y se hizo clic fuera de él, hacer zoom out
                Debug.Log($"Click fuera del objeto {gameObject.name} - Haciendo zoom out");
                ZoomOut();
            }
        }
    }

    private void HandleClick()
    {
        // Si este objeto ya está con zoom
        if (isZoomed)
        {
            // Segundo clic en el mismo objeto: activar la interfaz y hacer zoom out
            if (!IsInterfaceActive())
            {
                ActivateItemInterface();
                ZoomOut(false); // Hacer zoom out pero mantener la interfaz activa
            }
            else
            {
                // Si la interfaz ya está activa, cerrar todo
                DeactivateItemInterface();
                ZoomOut(true); // Cerrar interfaz y hacer zoom out
            }
        }
        else
        {
            // Si hay otro objeto con zoom, quitarle el zoom primero
            if (currentZoomedAsset != null && currentZoomedAsset != this)
            {
                currentZoomedAsset.ZoomOut();
            }

            // Primer clic: hacer zoom
            ZoomIn();
        }
    }

    private void OnMouseDown()
    {
        // Método alternativo por si el Update no funciona
        Debug.Log($"OnMouseDown llamado en {gameObject.name}");
        HandleClick();
    }

    /// <summary>
    /// Hace zoom in hacia este asset
    /// </summary>
    private void ZoomIn()
    {
        if (mainCamera == null) return;

        isZoomed = true;
        hasClickedOnce = true;  // Marcar que ya se hizo el primer clic
        currentZoomedAsset = this;

        // Ocultar el Canvas de navegación
        if (canvasReference != null)
        {
            canvasReference.SetActive(false);
        }

        // Guardar la posición original de la cámara
        originalCameraPosition = mainCamera.transform.position;

        // Calcular la posición target (mantener el Z de la cámara)
        targetCameraPosition = new Vector3(
            transform.position.x,
            transform.position.y,
            originalCameraPosition.z
        );

        // Iniciar la animación de zoom
        StopAllCoroutines();
        StartCoroutine(AnimateZoom(zoomedSize, targetCameraPosition));

        Debug.Log($"[{gameObject.name}] Primer clic: Zoom activado. Siguiente clic activará la interfaz del item.");
    }

    /// <summary>
    /// Hace zoom out y vuelve a la vista normal
    /// </summary>
    private void ZoomOut()
    {
        ZoomOut(true); // Por defecto desactiva la interfaz
    }

    /// <summary>
    /// Hace zoom out y vuelve a la vista normal
    /// </summary>
    /// <param name="deactivateInterface">Si debe desactivar la interfaz del item</param>
    private void ZoomOut(bool deactivateInterface)
    {
        if (mainCamera == null) return;

        isZoomed = false;
        hasClickedOnce = false;  // Reiniciar el estado de clics
        currentZoomedAsset = null;

        // Desactivar la interfaz del item solo si se especifica
        if (deactivateInterface)
        {
            DeactivateItemInterface();
        }

        // Mostrar el Canvas de navegación nuevamente
        if (canvasReference != null)
        {
            canvasReference.SetActive(true);
        }

        // Iniciar la animación de zoom out
        StopAllCoroutines();
        StartCoroutine(AnimateZoom(normalSize, originalCameraPosition));

        Debug.Log($"[{gameObject.name}] Zoom out: Volviendo a la vista normal.");
    }

    /// <summary>
    /// Anima el zoom de la cámara suavemente
    /// </summary>
    private System.Collections.IEnumerator AnimateZoom(float targetSize, Vector3 targetPosition)
    {
        float currentSize = mainCamera.orthographicSize;
        Vector3 currentPosition = mainCamera.transform.position;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * zoomSpeed;
            float t = Mathf.SmoothStep(0f, 1f, elapsed);

            // Interpolar el tamaño de la cámara
            mainCamera.orthographicSize = Mathf.Lerp(currentSize, targetSize, t);

            // Interpolar la posición de la cámara
            mainCamera.transform.position = Vector3.Lerp(currentPosition, targetPosition, t);

            yield return null;
        }

        // Asegurar que llega exactamente al valor final
        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = targetPosition;
    }

    /// <summary>
    /// Método público para hacer zoom out desde fuera (por ejemplo, desde un botón)
    /// </summary>
    public static void ResetZoom()
    {
        if (currentZoomedAsset != null)
        {
            currentZoomedAsset.ZoomOut();
        }
    }

    /// <summary>
    /// Verifica si la interfaz del item está activa
    /// </summary>
    private bool IsInterfaceActive()
    {
        return itemInterface != null && itemInterface.activeInHierarchy;
    }

    /// <summary>
    /// Activa la interfaz específica del item
    /// </summary>
    private void ActivateItemInterface()
    {
        if (itemInterface != null)
        {
            itemInterface.SetActive(true);
            Debug.Log($"[{gameObject.name}] Segundo clic: Interfaz del item activada.");
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] No se ha asignado una interfaz de item en el inspector.");
        }
    }

    /// <summary>
    /// Desactiva la interfaz específica del item
    /// </summary>
    private void DeactivateItemInterface()
    {
        if (itemInterface != null && itemInterface.activeInHierarchy)
        {
            itemInterface.SetActive(false);
            Debug.Log($"[{gameObject.name}] Interfaz del item desactivada.");
        }
    }
}
