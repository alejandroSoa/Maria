using UnityEngine;

/// <summary>
/// Controla el movimiento de las tuberías y se autodestruye al salir de la pantalla
/// </summary>
public class PipeMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de movimiento de la tubería")]
    [SerializeField] private float moveSpeed = 3f;
    
    [Tooltip("Posición X donde la tubería se destruye")]
    [SerializeField] private float destroyX = -12f;
    
    [Tooltip("Posición X donde se cuenta que el pájaro pasó la tubería")]
    [SerializeField] private float scorePositionX = -3f;

    private bool hasBeenPassed = false;
    private FlappyBirdGameManager gameManager;

    void Start()
    {
        // Buscar el GameManager
        gameManager = FindFirstObjectByType<FlappyBirdGameManager>();
        if (gameManager == null)
        {
            Debug.LogError("PipeMovement: No se encontró FlappyBirdGameManager en la escena!");
        }
    }

    void Update()
    {
        // Mover la tubería hacia la izquierda
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Verificar si el pájaro pasó la tubería
        if (!hasBeenPassed && transform.position.x < scorePositionX)
        {
            hasBeenPassed = true;
            if (gameManager != null)
            {
                gameManager.IncrementPipeCount();
                Debug.Log($"Tubería pasada en posición X: {transform.position.x}");
            }
        }

        // Destruir la tubería cuando salga de la pantalla
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Detiene el movimiento de la tubería
    /// </summary>
    public void StopMovement()
    {
        moveSpeed = 0f;
    }

    /// <summary>
    /// Cambia la velocidad de movimiento
    /// </summary>
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
