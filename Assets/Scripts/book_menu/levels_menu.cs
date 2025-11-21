using UnityEngine;
using TMPro;

public class levels_menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro1;
    [SerializeField] private TextMeshProUGUI textMeshPro2;
    [SerializeField] private TextMeshProUGUI[] textMeshProArray = new TextMeshProUGUI[3];
    
    private string level1Title;
    private string level1Description;

    void Start()
    {
        PlayerPrefs.SetString("Level_1", "done");
        PlayerPrefs.SetString("Level_2", "available");
        PlayerPrefs.SetString("Level_3", "available");
        PlayerPrefs.SetString("Level_4", "available");
        PlayerPrefs.Save();
        
        UpdateLevelContent();
    }
    
    void UpdateLevelContent()
    {
        string selectedLevel = PlayerPrefs.GetString("selectedlevel", "Level_1");
        
        string title = "";
        string description = "";
        
        switch (selectedLevel)
        {
            case "Level_1":
                title = "Bienvenida1";
                description = "Example1 Example Example Example Example Example Example Example Example Example Example Example Example Example";
                break;
            case "Level_2":
                title = "Bienvenida2";
                description = "Example2 Example Example Example Example Example Example Example Example Example Example Example Example Example";
                break;
            case "Level_3":
                title = "Bienvenida3";
                description = "Example3 Example Example Example Example Example Example Example Example Example Example Example Example Example";
                break;
            case "Level_4":
                title = "Bienvenida4";
                description = "Example4 Example Example Example Example Example Example Example Example Example Example Example Example Example";
                break;
            default:
                title = "Bienvenida1";
                description = "Example1 Example Example Example Example Example Example Example Example Example Example Example Example Example";
                break;
        }
        
        if (textMeshPro1 != null)
            textMeshPro1.text = title;
        
        if (textMeshPro2 != null)
            textMeshPro2.text = description;
    }


    void Update()
    {
        UpdateLevelContent();
    }
}
