using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string nextScene;   // almacena la escena que realmente quieres cargar

    public static void LoadWithLoadingScreen(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            nextScene = "Title"; // ← escena por default
        } else
        {
            nextScene = sceneName;
        }
        SceneManager.LoadScene("LoadingScene");    // ← tu escena de carga
    }
}
