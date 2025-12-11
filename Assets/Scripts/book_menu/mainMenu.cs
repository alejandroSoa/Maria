using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class mainMenu : MonoBehaviour
{
    public SoundManagerTitle sound;
    
    [Header("Panel References")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelsPanel;
    
    void Start()
    {
        // Verificar si venimos de un New Game
        bool isNewGame = PlayerPrefs.GetInt("IsNewGame", 0) == 1;
        
        if (isNewGame)
        {
            // Mostrar Levels y ocultar MainMenu
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (levelsPanel != null) levelsPanel.SetActive(true);
            
            // Resetear la bandera
            PlayerPrefs.SetInt("IsNewGame", 0);
            PlayerPrefs.Save();
            
        }
        else
        {
            // Mostrar MainMenu normalmente
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (levelsPanel != null) levelsPanel.SetActive(false);
        }
    }
    
    
    
    public void PlaySelect()
    {
        sound.PlaySelect();
    }

    public void LoadSelectedLevelScene()
    {
        string selectedLevel = PlayerPrefs.GetString("selectedlevel", "Level_1");
        
        // Si es Level_1, limpiar el inventario antes de cargar
        if (selectedLevel == "Level_1")
        {
            Inventory.ClearInventory();
        }
        
        string sceneToLoad = ConvertLevelToScene(selectedLevel);
        
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            //SceneManager.LoadScene(sceneToLoad);
            SceneLoader.LoadWithLoadingScreen(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No se pudo determinar la escena para el nivel: " + selectedLevel);
        }
    }
    
    private string ConvertLevelToScene(string levelName)
    {
        switch (levelName)
        {
            case "Level_1":
                return "Scenes/Room";
            case "Level_2":
                return "Scenes/Room_level2";  
            case "Level_3":
                return "Scenes/Room_level3";  
            case "Level_4":
                return "Scenes/Room_level4";    
            default:
                Debug.LogWarning("Nivel no reconocido: " + levelName);
                return "Scenes/Room"; 
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    /// <summary>
    /// Reinicia todos los niveles a estado "available" (nuevo juego)
    /// </summary>
    public void ResetAllLevels()
    {
        PlayerPrefs.SetString("Level_1", "available");
        PlayerPrefs.SetString("Level_2", "available");
        PlayerPrefs.SetString("Level_3", "available");
        PlayerPrefs.SetString("Level_4", "available");
        
        // Marcar que venimos de un New Game
        PlayerPrefs.SetInt("IsNewGame", 1);
        PlayerPrefs.Save();
                
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
