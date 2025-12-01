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

    void Update()
    {
        // Mover la tubería hacia la izquierda
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

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
