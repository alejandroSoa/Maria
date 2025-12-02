using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI decryptedCoinsText;
    
    [Header("Item Generation")]
    [SerializeField] public GameObject itemBaseTemplate; // Template base para copiar
    [SerializeField] public Transform itemsParent; // Padre donde se crean los items
    
    private const string DECRYPTED_COINS_KEY = "DecryptedCoins";
    
    void Start()
    {
        UpdateDecryptedCoinsDisplay();
    }
    
    void OnEnable()
    {
        UpdateDecryptedCoinsDisplay();
        
        // Generar items del inventario
        Invoke("GenerateInventoryItems", 0.1f); // Pequeño delay
    }

    void Update()
    {
        
    }
    
    /// <summary>
    /// Obtiene la cantidad actual de monedas descifradas desde SharedPreferences
    /// </summary>
    /// <returns>Cantidad de monedas descifradas</returns>
    public static int GetDecryptedCoins()
    {
        return PlayerPrefs.GetInt(DECRYPTED_COINS_KEY, 0);
    }
    
    /// <summary>
    /// Añade una moneda descifrada directamente en SharedPreferences
    /// </summary>
    public static void AddDecryptedCoin()
    {
        int currentCoins = PlayerPrefs.GetInt(DECRYPTED_COINS_KEY, 0);
        currentCoins++;
        
        PlayerPrefs.SetInt(DECRYPTED_COINS_KEY, currentCoins);
        PlayerPrefs.Save();
        
        Debug.Log($"Moneda descifrada obtenida! Total: {currentCoins}");
    }
    
    /// <summary>
    /// Actualiza el texto que muestra la cantidad de monedas descifradas
    /// </summary>
    private void UpdateDecryptedCoinsDisplay()
    {
        if (decryptedCoinsText != null)
        {
            int coins = GetDecryptedCoins();
            decryptedCoinsText.text = coins.ToString();
        }
    }
    
    /// <summary>
    /// Resetea las monedas descifradas
    /// </summary>
    public static void ResetDecryptedCoins()
    {
        PlayerPrefs.SetInt(DECRYPTED_COINS_KEY, 0);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Limpia completamente el inventario (monedas y todos los items)
    /// </summary>
    public static void ClearInventory()
    {
        Debug.Log("=== Limpiando inventario completo ===");
        
        // Resetear monedas descifradas
        PlayerPrefs.SetInt(DECRYPTED_COINS_KEY, 0);
        
        // Resetear todos los items
        string[] itemTypes = { "INT", "DATE", "VARCHAR", "BOOL" };
        foreach (string itemType in itemTypes)
        {
            string itemKey = "Item_" + itemType;
            PlayerPrefs.SetInt(itemKey, 0);
        }
        
        // Guardar cambios
        PlayerPrefs.Save();
        
        Debug.Log("Inventario limpiado completamente");
    }
    
    /// <summary>
    /// Obtiene la cantidad de un item específico del inventario
    /// </summary>
    /// <param name="itemType">Tipo de item (INT, DATE, VARCHAR, BOOL)</param>
    /// <returns>Cantidad del item</returns>
    public static int GetItemQuantity(string itemType)
    {
        string itemKey = "Item_" + itemType;
        return PlayerPrefs.GetInt(itemKey, 0);
    }
    
    /// <summary>
    /// Obtiene todos los items del inventario
    /// </summary>
    /// <returns>Diccionario con tipos de items y sus cantidades</returns>
    public static System.Collections.Generic.Dictionary<string, int> GetAllItems()
    {
        var items = new System.Collections.Generic.Dictionary<string, int>();
        string[] itemTypes = { "INT", "DATE", "VARCHAR", "BOOL" };
        
        foreach (string itemType in itemTypes)
        {
            int quantity = GetItemQuantity(itemType);
            if (quantity > 0)
            {
                items[itemType] = quantity;
            }
        }
        
        return items;
    }
    
    /// <summary>
    /// Obtiene los nombres en español de los items
    /// </summary>
    /// <returns>Diccionario con tipos de items y sus nombres en español</returns>
    public static System.Collections.Generic.Dictionary<string, string> GetItemNames()
    {
        return new System.Collections.Generic.Dictionary<string, string>()
        {
            { "INT", "Entero" },
            { "DATE", "Fecha" },
            { "VARCHAR", "Texto Variable" },
            { "BOOL", "Booleano" }
        };
    }
    
    /// <summary>
    /// Genera los items del inventario basándose en el template
    /// </summary>
    public void GenerateInventoryItems()
    {
        if (itemBaseTemplate == null || itemsParent == null)
        {
            Debug.LogWarning("Template o parent no asignados en Inventory");
            return;
        }
        
        Debug.Log("=== Generando items del inventario ===");
        
        // Limpiar items existentes
        ClearExistingItems();
        
        // Obtener items del inventario
        System.Collections.Generic.Dictionary<string, int> items = GetAllItems();
        System.Collections.Generic.Dictionary<string, string> itemNames = GetItemNames();
        
        Debug.Log($"Items encontrados: {items.Count}");
        
        if (items.Count == 0)
        {
            // Ocultar template si no hay items
            itemBaseTemplate.SetActive(false);
            return;
        }
        
        // Ocultar el template base
        itemBaseTemplate.SetActive(false);
        
        // Crear items
        foreach (var item in items)
        {
            CreateItemFromTemplate(item.Key, item.Value, itemNames[item.Key]);
        }
        
        Debug.Log($"Generados {items.Count} items");
    }
    
    /// <summary>
    /// Crea un item basándose en el template
    /// </summary>
    private void CreateItemFromTemplate(string itemType, int quantity, string displayName)
    {
        GameObject newItem = Instantiate(itemBaseTemplate, itemsParent);
        newItem.SetActive(true);
        newItem.name = $"Item_{itemType}";
        
        // Buscar textos y configurarlos
        TextMeshProUGUI[] texts = newItem.GetComponentsInChildren<TextMeshProUGUI>();
        
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.gameObject.name.ToLower().Contains("title"))
            {
                text.text = displayName;
            }
            else if (text.gameObject.name.ToLower().Contains("quantity"))
            {
                text.text = quantity.ToString();
            }
        }
        
        Debug.Log($"Item creado: {displayName} x{quantity}");
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
            if (child.gameObject != itemBaseTemplate && child.gameObject.name.StartsWith("Item_"))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
