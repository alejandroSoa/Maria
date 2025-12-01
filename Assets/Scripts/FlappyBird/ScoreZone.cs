using UnityEngine;

/// <summary>
/// Zona de puntuación que detecta cuando el jugador pasa por una tubería
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ScoreZone : MonoBehaviour
{
    private FlappyBirdGameManager gameManager;
    private bool hasScored = false;

    void Start()
    {
        // Buscar el GameManager
        gameManager = FindFirstObjectByType<FlappyBirdGameManager>();
        
        // Asegurar que el collider sea trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si es el jugador y no ha puntuado ya
        if (other.CompareTag("Player") && !hasScored)
        {
            hasScored = true;
            
            // Incrementar contador de tuberías
            if (gameManager != null)
            {
                gameManager.IncrementPipeCount();
            }
            
            Debug.Log("¡Tubería cruzada!");
        }
    }
}