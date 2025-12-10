using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona el estado del juego Flappy Bird y regreso a la escena principal
/// </summary>
public class FlappyBirdGameManager : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string mainRoomSceneName = "Room";
    [SerializeField] private float delayBeforeReturn = 1f;

    [Header("Referencias del Juego")]
    [SerializeField] private FlappyBirdPlayer player;
    [SerializeField] private PipeSpawner pipeSpawner;
    
    [Header("Configuración de Puntuación")]
    [SerializeField] private int pipesPerCoin = 1; // Tuberías por moneda
    [SerializeField] private TMPro.TextMeshProUGUI pipeCounterText; // Texto UI para mostrar contador

    private bool isGameOver = false;
    private int pipesPassed = 0;

    void Start()
    {
        // Buscar referencias si no están asignadas
        if (player == null)
        {
            player = FindFirstObjectByType<FlappyBirdPlayer>();
        }

        if (pipeSpawner == null)
        {
            pipeSpawner = FindFirstObjectByType<PipeSpawner>();
        }
        
        // Buscar texto UI si no está asignado
        if (pipeCounterText == null)
        {
            pipeCounterText = FindFirstObjectByType<TMPro.TextMeshProUGUI>();
        }
        
        // Inicializar contador
        pipesPassed = 0;
        UpdatePipeCounter();
    }

    /// <summary>
    /// Maneja el estado de Game Over
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("¡Game Over! Regresando a Room...");

        // Detener el spawn de tuberías
        if (pipeSpawner != null)
        {
            pipeSpawner.StopSpawning();
        }

        // Detener todas las tuberías
        PipeMovement[] pipes = FindObjectsOfType<PipeMovement>();
        foreach (PipeMovement pipe in pipes)
        {
            pipe.StopMovement();
        }

        // Regresar a la escena principal después de un pequeño delay
        Invoke(nameof(ReturnToMainRoom), delayBeforeReturn);
    }

    /// <summary>
    /// Método para salir manualmente del juego (puede ser llamado por un botón)
    /// </summary>
    public void ExitGame()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        
        // Detener el spawn de tuberías
        if (pipeSpawner != null)
        {
            pipeSpawner.StopSpawning();
        }
        
        // Detener todas las tuberías
        PipeMovement[] pipes = FindObjectsOfType<PipeMovement>();
        foreach (PipeMovement pipe in pipes)
        {
            pipe.StopMovement();
        }
        
        // Regresar a la escena principal después de un pequeño delay
        Invoke(nameof(ReturnToMainRoom), delayBeforeReturn);
    }
    
    /// <summary>
    /// Incrementa el contador cuando el jugador pasa una tubería
    /// </summary>
    public void IncrementPipeCount()
    {
        if (isGameOver) return;
        
        pipesPassed++;
        Debug.Log($"Tuberías pasadas: {pipesPassed}");
        
        // Verificar si debe otorgar moneda (cada 10 tuberías)
        if (pipesPassed % pipesPerCoin == 0)
        {
            Inventory.AddDecryptedCoin();
        }
        
        // Actualizar contador en UI
        UpdatePipeCounter();
    }
    
    /// <summary>
    /// Actualiza el texto UI que muestra el contador de tuberías
    /// </summary>
    private void UpdatePipeCounter()
    {
        if (pipeCounterText != null)
        {
            pipeCounterText.text = pipesPassed.ToString();
        }
        else
        {
        }
    }
    
    /// <summary>
    /// Vuelve a la escena principal (Room)
    /// </summary>
    private void ReturnToMainRoom()
    {
        Debug.Log($"Volviendo a la escena: {mainRoomSceneName}");
        SceneManager.LoadScene(mainRoomSceneName);
    }
}
