using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marianet : MonoBehaviour
{
    [SerializeField] private Button buttonBuyInt;
    [SerializeField] private Button buttonBuyDate;
    [SerializeField] private Button buttonBuyVarchar;
    [SerializeField] private Button buttonBuyBool;

    public SoundManagerRoom soundRoom;

    public TextMeshProUGUI koensText;
    
    [Header("Fusible Sprite System")]
    [SerializeField] private GameObject fusibleSpritePrefab;
    [SerializeField] private Transform spawnArea;
    
    // Diccionario de items comprados con sus cantidades
    private const string ITEMS_KEY_PREFIX = "Item_";
    

    
    // Nombres de los items en español
    private readonly System.Collections.Generic.Dictionary<string, string> itemNames = new System.Collections.Generic.Dictionary<string, string>()
    {
        { "INT", "Entero" },
        { "DATE", "Fecha" },
        { "VARCHAR", "Texto Variable" },
        { "BOOL", "Booleano" }
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonBuyInt.onClick.AddListener(() => OnBuyButtonClicked("INT"));
        buttonBuyDate.onClick.AddListener(() => OnBuyButtonClicked("DATE"));
        buttonBuyVarchar.onClick.AddListener(() => OnBuyButtonClicked("VARCHAR"));
        buttonBuyBool.onClick.AddListener(() => OnBuyButtonClicked("BOOL"));

        UpdateCoinsText();
    }

    void OnBuyButtonClicked(string tipo)
    {
        int currentCoins = Inventory.GetDecryptedCoins();
        
        if (currentCoins <= 0)
        {
            soundRoom.PlayPurchaseError();
            Debug.Log($"No tienes monedas suficientes para comprar {tipo}");
        }
        else
        {
            // Gastar una moneda descifrada
            int newCoinAmount = currentCoins - 1;
            PlayerPrefs.SetInt("DecryptedCoins", newCoinAmount);
            PlayerPrefs.Save();
            
            // Crear sprite de fusible en lugar de agregar directamente al inventario
            CreateFusibleSprite(tipo);
            
            soundRoom.PlayPurchase();
            Debug.Log($"Fusible {itemNames[tipo]} comprado. Haz click en el sprite para recogerlo. Monedas restantes: {newCoinAmount}");
            
            UpdateCoinsText();
        }
    }

    void UpdateCoinsText()
    {
        if (koensText != null)
        {
            int coins = Inventory.GetDecryptedCoins();
            koensText.text = coins.ToString();
        }
    }
    
    /// <summary>
    /// Crea un sprite clickeable que representa el/los fusibles comprados
    /// </summary>
    /// <param name="itemType">Tipo de fusible comprado</param>
    private void CreateFusibleSprite(string itemType)
    {
        if (fusibleSpritePrefab == null || spawnArea == null)
        {
            Debug.LogError("Fusible sprite prefab o spawn area no están asignados");
            AddFusibleToInventory(itemType); // Fallback: agregar directamente
            return;
        }
        
        // Crear el sprite
        GameObject fusibleSprite = Instantiate(fusibleSpritePrefab, spawnArea);
        
        // Configurar el item fusible
        FusibleItem fusibleItem = fusibleSprite.GetComponent<FusibleItem>();
        if (fusibleItem == null)
        {
            fusibleItem = fusibleSprite.AddComponent<FusibleItem>();
        }
        
        // Inicializar con el tipo de fusible y referencia a Marianet
        System.Collections.Generic.List<string> types = new System.Collections.Generic.List<string> { itemType };
        fusibleItem.Initialize(types, this);
        fusibleItem.SetRandomPosition(spawnArea);
        
        Debug.Log($"Sprite de fusible {itemNames[itemType]} creado. Haz click para recogerlo.");
    }
    

    
    /// <summary>
    /// Agrega un fusible al inventario guardándolo en PlayerPrefs
    /// </summary>
    /// <param name="itemType">Tipo de item (INT, DATE, VARCHAR, BOOL)</param>
    public void AddFusibleToInventory(string itemType)
    {
        string itemKey = ITEMS_KEY_PREFIX + itemType;
        int currentQuantity = PlayerPrefs.GetInt(itemKey, 0);
        currentQuantity++;
        
        PlayerPrefs.SetInt(itemKey, currentQuantity);
        PlayerPrefs.Save();
        
        Debug.Log($"Item agregado al inventario: {itemNames[itemType]} x{currentQuantity}");
    }
    
    /// <summary>
    /// Obtiene la cantidad de un item en el inventario
    /// </summary>
    /// <param name="itemType">Tipo de item</param>
    /// <returns>Cantidad del item</returns>
    public static int GetItemQuantity(string itemType)
    {
        string itemKey = ITEMS_KEY_PREFIX + itemType;
        return PlayerPrefs.GetInt(itemKey, 0);
    }
    
    /// <summary>
    /// Obtiene el nombre en español de un item
    /// </summary>
    /// <param name="itemType">Tipo de item</param>
    /// <returns>Nombre en español</returns>
    public string GetItemName(string itemType)
    {
        return itemNames.ContainsKey(itemType) ? itemNames[itemType] : itemType;
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizar texto de monedas periódicamente en caso de que cambien
        UpdateCoinsText();
    }
}
