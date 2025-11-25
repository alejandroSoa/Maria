using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ToggleObjectsOnClick : MonoBehaviour
{
    [Header("Toggle GameObjects")]
    [Tooltip("Objetos que se mostrarán en el primer estado")]
    public GameObject objects1;
    [Tooltip("Objetos que se mostrarán en el segundo estado")]
    public GameObject objects2;
    
    [Header("Movement Interface")]
    [Tooltip("Interfaz de movimiento que se controla según el estado")]
    public GameObject movement;
    
    [Header("Referencias")]
    [SerializeField] private Camera mainCamera;
    
    private void Start()
    {
        // Configurar el Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
            rb.simulated = true;
        }

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = false;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError($"Door_Room en {gameObject.name}: No se encontró la Main Camera.");
        }
        
        ShowObjects1();
        
        Debug.Log($"[{gameObject.name}] ToggleObjectsOnClick inicializado correctamente");
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return; 
            }

            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log($"¡CLICK DETECTADO en {gameObject.name}!");
                HandleClick();
            }
        }
    }
    
    private void HandleClick()
    {
        if (objects1 != null && objects1.activeInHierarchy)
        {
            Debug.Log("Objects1 detectado activo - Cambiando a Objects2");
            ShowObjects2();
        }
        else if (objects2 != null && objects2.activeInHierarchy)
        {
            Debug.Log("Objects2 detectado activo - Cambiando a Objects1");
            ShowObjects1();
        }
        else
        {
            ShowObjects1();
        }
    }
    
    public void OnClick(bool showFirstObjects)
    {
        Debug.Log($"OnClick called with value: {showFirstObjects}");
        
        if (showFirstObjects)
        {
            ShowObjects1();
        }
        else
        {
            ShowObjects2();
        }
    }
    
    private void ShowObjects1()
    {
        Debug.Log("Mostrando Objects1");
        
        if (objects1 != null) 
        {
            objects1.SetActive(true);
            Debug.Log("Objects1 activado");
        }
        if (objects2 != null) 
        {
            objects2.SetActive(false);
            Debug.Log("Objects2 desactivado");
        }
        if (movement != null) 
        {
            movement.SetActive(true);
            Debug.Log("Movement activado");
        }
    }
    
    private void ShowObjects2()
    {
        Debug.Log("Mostrando Objects2");
        
        if (objects1 != null) 
        {
            objects1.SetActive(false);
            Debug.Log("Objects1 desactivado");
        }
        if (objects2 != null) 
        {
            objects2.SetActive(true);
            Debug.Log("Objects2 activado");
        }
        if (movement != null) 
        {
            movement.SetActive(false);
            Debug.Log("Movement desactivado");
        }
    }
}
