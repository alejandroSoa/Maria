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
        level1Title = PlayerPrefs.GetString("Level1Title", "Bienvenida");
        level1Description = PlayerPrefs.GetString("Level1Description", "Example Example Example Example Example Example Example Example Example Example Example Example Example Example");
        
        PlayerPrefs.SetString("Level1Title", level1Title);
        PlayerPrefs.SetString("Level1Description", level1Description);
        PlayerPrefs.Save();
        
        if (textMeshPro1 != null)
            textMeshPro1.text = level1Title;
        
        if (textMeshPro2 != null)
            textMeshPro2.text = level1Description;
    }


    void Update()
    {
        
    }
}
