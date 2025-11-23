using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class mainMenu : MonoBehaviour
{
    public SoundManagerTitle sound;
    
    
    
    public void PlaySelect()
    {
        sound.PlaySelect();
    }

    public void LoadSelectedLevelScene()
    {
        string selectedLevel = PlayerPrefs.GetString("selectedlevel", "Level_1");
        string sceneToLoad = ConvertLevelToScene(selectedLevel);
        
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
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
}
