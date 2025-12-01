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

    private bool isGameOver = false;

    void Start()
    {
        // Buscar referencias si no están asignadas
        if (player == null)
        {
            player = FindObjectOfType<FlappyBirdPlayer>();
        }

        if (pipeSpawner == null)
        {
            pipeSpawner = FindObjectOfType<PipeSpawner>();
        }
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
    /// Vuelve a la escena principal (Room)
    /// </summary>
    private void ReturnToMainRoom()
    {
        Debug.Log($"Volviendo a la escena: {mainRoomSceneName}");
        SceneManager.LoadScene(mainRoomSceneName);
    }
}
