using UnityEngine;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TMP_Text instructionText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Configurar texto de instrucción hardcodeado
        if (instructionText != null)
        {
            instructionText.text = "Obtener todos los valores de la tabla Users.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
