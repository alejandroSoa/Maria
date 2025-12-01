using UnityEngine;
using TMPro;

/// <summary>
/// Maneja el inventario en modo edición (selección) con signos de exclamación
/// </summary>
public class InventoryEditMode : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI decryptedCoinsText; // Texto para mostrar monedas
    
    [Header("Item Generation")]
    [SerializeField] public GameObject itemBaseTemplate; // Template base para copiar
    [SerializeField] public Transform itemsParent; // Padre donde se crean los items
    
    [Header("References")]
    [SerializeField] private Fusebox fuseboxReference; // Referencia al fusebox
    
    void Start()
    {
        UpdateDecryptedCoinsDisplay();
        GenerateEditModeItems();
    }
    
    void OnEnable()
    {
        // Actualizar monedas y regenerar items cada vez que se active
        UpdateDecryptedCoinsDisplay();
        Invoke("GenerateEditModeItems", 0.1f);
    }
    
    /// <summary>
    /// Genera los items del inventario en modo edición
    /// </summary>
    public void GenerateEditModeItems()
    {
        if (itemBaseTemplate == null || itemsParent == null)
        {
            Debug.LogWarning("Template o parent no asignados en InventoryEditMode");
            return;
        }
        
        Debug.Log("=== Generando inventario en modo edición ===");
        
        // Limpiar items existentes
        ClearExistingItems();
        
        // Obtener items del inventario
        System.Collections.Generic.Dictionary<string, int> items = Inventory.GetAllItems();
        System.Collections.Generic.Dictionary<string, string> itemNames = Inventory.GetItemNames();
        
        if (items.Count == 0)
        {
            // Ocultar template si no hay items
            itemBaseTemplate.SetActive(false);
            return;
        }
        
        // Ocultar el template base
        itemBaseTemplate.SetActive(false);
        
        // Crear items con signos de exclamación
        foreach (var item in items)
        {
            if (item.Value > 0) // Solo mostrar items con cantidad > 0
            {
                CreateEditModeItemFromTemplate(item.Key, item.Value, itemNames[item.Key]);
            }
        }
        
        Debug.Log($"Generados {items.Count} items en modo edición");
    }
    
    /// <summary>
    /// Crea un item en modo edición basándose en el template
    /// </summary>
    private void CreateEditModeItemFromTemplate(string itemType, int quantity, string displayName)
    {
        GameObject newItem = Instantiate(itemBaseTemplate, itemsParent);
        newItem.SetActive(true);
        newItem.name = $"EditItem_{itemType}";
        
        // Buscar textos y configurarlos
        TextMeshProUGUI[] texts = newItem.GetComponentsInChildren<TextMeshProUGUI>();
        
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.gameObject.name.ToLower().Contains("title"))
            {
                // Agregar signo de exclamación al nombre
                text.text = "! " + displayName;
                text.color = Color.yellow; // Cambiar color para indicar modo selección
            }
            else if (text.gameObject.name.ToLower().Contains("quantity"))
            {
                text.text = quantity.ToString();
            }
        }
        
        // Buscar y configurar botón para selección
        UnityEngine.UI.Button[] buttons = newItem.GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (UnityEngine.UI.Button button in buttons)
        {
            string itemRef = itemType; // Captura para closure
            button.onClick.RemoveAllListeners(); // Limpiar listeners previos
            button.onClick.AddListener(() => OnItemSelectedForAssignment(itemRef));
            break; // Solo el primer botón
        }
        
        Debug.Log($"Item modo edición creado: {displayName} x{quantity}");
    }
    
    /// <summary>
    /// Se llama cuando se selecciona un item para asignación
    /// </summary>
    private void OnItemSelectedForAssignment(string itemType)
    {
        Debug.Log($"Item seleccionado para asignación: {itemType}");
        
        // Verificar que el item tenga cantidad disponible
        int quantity = Inventory.GetItemQuantity(itemType);
        if (quantity <= 0)
        {
            Debug.Log("Este item no tiene cantidad disponible");
            return;
        }
        
        // Notificar al fusebox que se seleccionó un item
        if (fuseboxReference != null)
        {
            fuseboxReference.OnFuseSelected(itemType);
        }
        else
        {
            // Buscar fusebox si no está asignado
            Fusebox fusebox = FindFirstObjectByType<Fusebox>();
            if (fusebox != null)
            {
                fusebox.OnFuseSelected(itemType);
            }
        }
    }
    
    /// <summary>
    /// Limpia items existentes
    /// </summary>
    private void ClearExistingItems()
    {
        if (itemsParent == null) return;
        
        for (int i = itemsParent.childCount - 1; i >= 0; i--)
        {
            Transform child = itemsParent.GetChild(i);
            if (child.gameObject != itemBaseTemplate && child.gameObject.name.StartsWith("EditItem_"))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
    
    /// <summary>
    /// Actualiza el texto que muestra la cantidad de monedas descifradas
    /// </summary>
    private void UpdateDecryptedCoinsDisplay()
    {
        if (decryptedCoinsText != null)
        {
            int coins = Inventory.GetDecryptedCoins();
            decryptedCoinsText.text = coins.ToString();
        }
    }
    
    /// <summary>
    /// Método público para actualizar el inventario en modo edición
    /// </summary>
    public void RefreshEditModeDisplay()
    {
        UpdateDecryptedCoinsDisplay();
        GenerateEditModeItems();
    }
}