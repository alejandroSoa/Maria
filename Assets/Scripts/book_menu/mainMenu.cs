using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class mainMenu : MonoBehaviour
{
    public SoundManagerTitle sound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlaySelect()
    {
        sound.PlaySelect();
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
