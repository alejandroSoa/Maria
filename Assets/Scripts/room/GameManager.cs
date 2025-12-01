using UnityEngine;

/// <summary>
/// Administrador global del juego que mantiene referencias importantes
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    public Inventory inventory;
    
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Buscar inventario si no está asignado
        if (inventory == null)
        {
            inventory = FindFirstObjectByType<Inventory>();
        }
    }
    
    /// <summary>
    /// Obtiene la referencia del inventario
    /// </summary>
    /// <returns>Instancia del inventario</returns>
    public Inventory GetInventory()
    {
        if (inventory == null)
        {
            inventory = FindFirstObjectByType<Inventory>();
        }
        return inventory;
    }
}