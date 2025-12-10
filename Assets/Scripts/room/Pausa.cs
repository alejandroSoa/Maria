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
    /// Carga la escena Title (escena 0)
    /// </summary>
    public void LoadTitleScene()
    {
        SceneManager.LoadScene(0);
    }
}
