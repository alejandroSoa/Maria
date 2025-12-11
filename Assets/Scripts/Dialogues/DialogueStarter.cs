using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueStarter : MonoBehaviour
{
    public DialogueController controller;

    void Start()
    {
        // Obtener el nivel actual del jugador
        int currentLevel = GetCurrentLevel();
        
        // Cargar los diálogos correspondientes al nivel
        controller.StartDialogueForLevel(currentLevel);
        
        Debug.Log($"[DialogueStarter] Escena: '{SceneManager.GetActiveScene().name}' -> Cargando diálogos para nivel: {currentLevel}");
    }
    
    private int GetCurrentLevel()
    {
        // Primero intentar obtener desde el nombre de la escena actual
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // Si la escena se llama Level_2, Level_3, etc.
        if (currentSceneName.StartsWith("Level_"))
        {
            string[] parts = currentSceneName.Split('_');
            if (parts.Length > 1 && int.TryParse(parts[1], out int sceneLevel))
            {
                if (sceneLevel >= 1 && sceneLevel <= 4)
                {
                    Debug.Log($"[DialogueStarter] Nivel detectado desde escena: {sceneLevel}");
                    return sceneLevel;
                }
            }
        }
        
        // Detectar formato "Room_levelX" o "room_levelX"
        if (currentSceneName.ToLower().Contains("level"))
        {
            // Buscar el número después de "level"
            int levelIndex = currentSceneName.ToLower().IndexOf("level") + 5;
            if (levelIndex < currentSceneName.Length)
            {
                string numberPart = "";
                for (int i = levelIndex; i < currentSceneName.Length; i++)
                {
                    if (char.IsDigit(currentSceneName[i]))
                    {
                        numberPart += currentSceneName[i];
                    }
                    else
                    {
                        break;
                    }
                }
                
                if (int.TryParse(numberPart, out int levelNum))
                {
                    if (levelNum >= 1 && levelNum <= 4)
                    {
                        Debug.Log($"[DialogueStarter] Nivel detectado desde escena (formato Room_level): {levelNum}");
                        return levelNum;
                    }
                }
            }
        }
        
        // Si no, usar PlayerPrefs como respaldo
        string selectedLevel = PlayerPrefs.GetString("selectedlevel", "Level_1");
        Debug.Log($"[DialogueStarter] selectedlevel en PlayerPrefs: '{selectedLevel}'");
        
        // Extraer el número del nivel (Level_1 -> 1, Level_2 -> 2, etc.)
        if (selectedLevel.Contains("_"))
        {
            string[] parts = selectedLevel.Split('_');
            if (parts.Length > 1 && int.TryParse(parts[1], out int levelNum))
            {
                // Validar que el nivel esté entre 1 y 4
                if (levelNum >= 1 && levelNum <= 4)
                {
                    return levelNum;
                }
            }
        }
        
        // Por defecto, retornar nivel 1
        Debug.Log("[DialogueStarter] Usando nivel por defecto: 1");
        return 1;
    }
}

