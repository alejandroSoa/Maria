using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Script para manejar un item fusible que aparece cuando el jugador compra algo
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FusibleItem : MonoBehaviour
{
    [Header("Fusible Configuration")]
    public List<string> fusibleTypes = new List<string>();
    
    [Header("Visual Configuration")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Camera mainCamera;
    
    // Referencias
    private Marianet marianetScript;
    
    void Start()
    {
        InitializeFusibleItem();
    }
    
    void Update()
    {
        DetectClicks();
    }
    
    private void InitializeFusibleItem()
    {
        // Configurar cámara
        if (mainCamera == null) mainCamera = Camera.main;
        
        // Configurar componentes si no existen
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        SetupCollider();
        SetupRigidbody();
        
        // Configurar para tener prioridad sobre otros clickables
        SetupPriority();
        
        // Encontrar el script de Marianet en la escena solo si no está asignado
        if (marianetScript == null)
        {
            marianetScript = FindObjectOfType<Marianet>();
        }
    }
    
    private void SetupPriority()
    {
        // Mover el fusible a una posición Z más cercana a la cámara para tener prioridad
        Vector3 pos = transform.position;
        pos.z = -1f; // Más cerca de la cámara que otros objetos
        transform.position = pos;
        
        // Asegurar que el sorting layer tenga prioridad
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 100; // Orden alto para estar encima
        }
    }
    
    private void SetupCollider()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = false;
        }
    }
    
    private void SetupRigidbody()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
            rb.simulated = true;
        }
    }
    
    private void DetectClicks()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            // Hacer raycast específico para fusibles primero
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
            
            // Buscar si algún hit es este fusible
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // Encontramos el fusible, recolectarlo inmediatamente
                    DialogueController.Instance.ResumeDialogue();
                    CollectFusibles();
                    return; // Salir para evitar que otros objetos procesen el click
                }
            }
        }
    }
    
    /// <summary>
    /// Inicializa el fusible con los tipos especificados
    /// </summary>
    /// <param name="types">Tipos de fusibles que contiene este item</param>
    /// <param name="marianet">Referencia al script Marianet</param>
    public void Initialize(List<string> types, Marianet marianet)
    {
        fusibleTypes = new List<string>(types);
        marianetScript = marianet;
    }
    
    /// <summary>
    /// Agrega un tipo de fusible al item
    /// </summary>
    /// <param name="fusibleType">Tipo de fusible a agregar</param>
    public void AddFusible(string fusibleType)
    {
        fusibleTypes.Add(fusibleType);
    }
    
    /// <summary>
    /// Método de respaldo para clicks (OnMouseDown)
    /// </summary>
    private void OnMouseDown()
    {
        DialogueController.Instance.ResumeDialogue();
        CollectFusibles();
    }
    
    /// <summary>
    /// Recolecta todos los fusibles y los envía al inventario
    /// </summary>
    public void CollectFusibles()
    {
        Debug.Log($"CollectFusibles llamado. marianetScript null? {marianetScript == null}, fusibleTypes count: {fusibleTypes.Count}");
        
        if (marianetScript == null || fusibleTypes.Count == 0)
        {
            Debug.LogError($"No se puede recolectar fusible. marianetScript: {marianetScript}, fusibleTypes.Count: {fusibleTypes.Count}");
            return;
        }
            
        // Enviar cada fusible al inventario
        foreach (string fusibleType in fusibleTypes)
        {
            Debug.Log($"Agregando fusible tipo: {fusibleType}");
            marianetScript.AddFusibleToInventory(fusibleType);
        }
        
        // Mostrar mensaje de recolección
        string fusibleNames = string.Join(", ", fusibleTypes.ConvertAll(t => marianetScript.GetItemName(t)));
        Debug.Log($"Fusibles recolectados: {fusibleNames}");
        
        // Destruir el item
        Debug.Log("Destruyendo objeto fusible");
        Destroy(gameObject);
        DialogueController.Instance.ResumeDialogue();
    }
    
    /// <summary>
    /// Establece la posición exacta del item en el área de spawn
    /// </summary>
    /// <param name="spawnArea">Área donde aparecerá el item</param>
    public void SetRandomPosition(Transform spawnArea)
    {
        if (spawnArea == null) return;
        
        // Usar la posición exacta del spawnArea sin offset aleatorio
        Vector3 exactPosition = spawnArea.position;
        exactPosition.z = -1f; // Solo ajustar Z para estar delante
        
        transform.position = exactPosition;
        transform.SetParent(spawnArea); // Mantener jerarquía
        
        Debug.Log($"Fusible spawneado en posición exacta: {transform.position}");
    }
}