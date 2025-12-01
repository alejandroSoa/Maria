using UnityEngine;

/// <summary>
/// Genera tuberías a intervalos regulares para el minijuego Flappy Bird
/// </summary>
public class PipeSpawner : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    [Tooltip("Prefab de la tubería a instanciar")]
    [SerializeField] private GameObject pipePrefab;
    
    [Tooltip("Tiempo entre spawns de tuberías (en segundos)")]
    [SerializeField] private float spawnInterval = 2f;
    
    [Tooltip("Altura mínima de spawn")]
    [SerializeField] private float minHeight = -2f;
    
    [Tooltip("Altura máxima de spawn")]
    [SerializeField] private float maxHeight = 2f;
    
    [Tooltip("Posición X donde aparecen las tuberías")]
    [SerializeField] private float spawnX = 10f;

    [Header("Estado")]
    [Tooltip("Si está activado, genera tuberías automáticamente")]
    [SerializeField] private bool isSpawning = true;

    private float spawnTimer = 0f;

    void Update()
    {
        if (!isSpawning) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnPipe();
            spawnTimer = 0f;
        }
    }

    void SpawnPipe()
    {
        if (pipePrefab == null)
        {
            Debug.LogWarning("PipeSpawner: No se ha asignado el prefab de tubería");
            return;
        }

        // Calcular una altura aleatoria
        float randomHeight = Random.Range(minHeight, maxHeight);

        // Crear la posición de spawn
        Vector3 spawnPosition = new Vector3(spawnX, randomHeight, 0f);

        // Instanciar la tubería
        GameObject newPipe = Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
        
        Debug.Log($"Tubería generada en Y: {randomHeight}");
    }

    /// <summary>
    /// Inicia el spawn de tuberías
    /// </summary>
    public void StartSpawning()
    {
        isSpawning = true;
        spawnTimer = 0f;
    }

    /// <summary>
    /// Detiene el spawn de tuberías
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
    }

    /// <summary>
    /// Limpia todas las tuberías de la escena
    /// </summary>
    public void ClearPipes()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject pipe in pipes)
        {
            Destroy(pipe);
        }
    }
}
