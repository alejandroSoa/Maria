using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public DialogueController controller;

    void Start()
    {
        // Obtener el nivel actual del jugador desde PlayerPrefs
        int currentLevel = GetCurrentLevel();
        
        // Cargar los diálogos correspondientes al nivel
        controller.StartDialogueForLevel(currentLevel);
        
        string selectedLevelKey = PlayerPrefs.GetString("selectedlevel", "Level_1");
        Debug.Log($"[DialogueStarter] selectedlevel en PlayerPrefs: '{selectedLevelKey}' -> Cargando diálogos para nivel: {currentLevel}");
    }
    
    private int GetCurrentLevel()
    {
        // Obtener el nivel desde "selectedlevel" en PlayerPrefs
        string selectedLevel = PlayerPrefs.GetString("selectedlevel", "Level_1");
        
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
        return 1;
    }
}

