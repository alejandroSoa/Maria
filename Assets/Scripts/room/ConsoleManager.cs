using UnityEngine;
using TMPro;

/// <summary>
/// Maneja la consola SQL - versión simplificada con instrucción hardcodeada
/// </summary>
public class ConsoleManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TMP_Text instructionText;

    void Start()
    {
        // Configurar texto de instrucción hardcodeado
        if (instructionText != null)
        {
            instructionText.text = "Obtener todos los valores de la tabla Users.";
        }
    }
}
