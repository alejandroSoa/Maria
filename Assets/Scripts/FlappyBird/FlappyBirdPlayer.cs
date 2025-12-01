using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla el comportamiento del jugador en el minijuego Flappy Bird
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class FlappyBirdPlayer : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    [Tooltip("Fuerza del salto/aleteo")]
    [SerializeField] private float jumpForce = 5f;
    
    [Tooltip("Gravedad aplicada al jugador")]
    [SerializeField] private float gravity = 2f;
    
    [Tooltip("Velocidad máxima de caída")]
    [SerializeField] private float maxFallSpeed = -10f;
    
    [Tooltip("Rotación máxima hacia arriba")]
    [SerializeField] private float maxUpRotation = 30f;
    
    [Tooltip("Rotación máxima hacia abajo")]
    [SerializeField] private float maxDownRotation = -90f;
    
    [Tooltip("Velocidad de rotación")]
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Referencias")]
    [SerializeField] private FlappyBirdGameManager gameManager;
    [SerializeField] private Camera mainCamera;

    private Rigidbody2D rb;
    private bool isAlive = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Configurar el Rigidbody2D
        rb.gravityScale = gravity;
        
        // Buscar el GameManager si no está asignado
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<FlappyBirdGameManager>();
        }
        
        // Buscar la cámara principal si no está asignada
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (!isAlive) return;

        // Detectar input de salto
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Jump();
        }
        else if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }

        // Limitar velocidad de caída
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }

        // Rotar el jugador basado en la velocidad vertical
        RotatePlayer();
        
        // Verificar si el pájaro se salió de la pantalla por abajo
        CheckOutOfBounds();
    }

    void Jump()
    {
        // Resetear la velocidad vertical y aplicar fuerza de salto
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void RotatePlayer()
    {
        // Calcular la rotación basada en la velocidad vertical
        float targetRotation;
        
        if (rb.velocity.y > 0)
        {
            // Subiendo - rotar hacia arriba
            targetRotation = Mathf.Lerp(0, maxUpRotation, rb.velocity.y / jumpForce);
        }
        else
        {
            // Cayendo - rotar hacia abajo
            targetRotation = Mathf.Lerp(0, maxDownRotation, -rb.velocity.y / Mathf.Abs(maxFallSpeed));
        }

        // Aplicar la rotación suavemente
        Quaternion targetQuaternion = Quaternion.Euler(0, 0, targetRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, rotationSpeed * Time.deltaTime);
    }
    
    void CheckOutOfBounds()
    {
        if (mainCamera == null) return;
        
        // Obtener el límite inferior de la cámara en coordenadas del mundo
        float bottomBound = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        
        // Si el pájaro está por debajo del límite inferior, muere
        if (transform.position.y < bottomBound - 1f) // -1f para dar un margen
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlive) return;

        // Detectar colisión con tuberías
        if (collision.CompareTag("Pipe") || collision.CompareTag("Ground"))
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;

        // Detectar colisión con cualquier obstáculo
        if (collision.gameObject.CompareTag("Pipe") || collision.gameObject.CompareTag("Ground"))
        {
            Die();
        }
    }

    void Die()
    {
        if (!isAlive) return;

        isAlive = false;
        Debug.Log("El jugador ha muerto");

        // Cambiar el color del sprite para feedback visual
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        // Notificar al GameManager
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    /// <summary>
    /// Reinicia el estado del jugador
    /// </summary>
    public void ResetPlayer()
    {
        isAlive = true;
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }

    /// <summary>
    /// Detiene el movimiento del jugador
    /// </summary>
    public void StopPlayer()
    {
        isAlive = false;
        rb.velocity = Vector2.zero;
    }
}
