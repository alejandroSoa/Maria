using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausa : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Carga la escena Title (menú de selección de niveles)
    /// </summary>
    public void LoadTitleScene()
    {
        // Limpiar el nivel seleccionado para que no se quede guardado
        PlayerPrefs.DeleteKey("selectedlevel");
        PlayerPrefs.Save();
        
        // Cargar la escena Title (escena 2 según EditorBuildSettings)
        SceneManager.LoadScene("Title");
    }
}
