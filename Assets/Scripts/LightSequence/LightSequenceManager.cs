using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightSequenceManager : MonoBehaviour
{
    public LightButton[] buttons; // 5 botones
    public LightBulb[] bulbs; // 5 focos
    
    private List<int> correctSequence = new List<int>();
    private int currentStep = 0;
    
    private bool gameCompleted = false;

    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        GenerateRandomSequence();
        InitializeBulbs();
        Debug.Log("Secuencia correcta: " + string.Join(" -> ", correctSequence));
    }

    void GenerateRandomSequence()
    {
        correctSequence.Clear();
        
        // Crear lista de botones disponibles
        List<int> availableButtons = new List<int> { 0, 1, 2, 3, 4 };
        
        // Seleccionar 5 botones sin repetir (todos los botones en orden aleatorio)
        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, availableButtons.Count);
            correctSequence.Add(availableButtons[randomIndex]);
            availableButtons.RemoveAt(randomIndex);
        }
    }

    void InitializeBulbs()
    {
        foreach (LightBulb bulb in bulbs)
        {
            bulb.TurnOff();
        }
    }

    public void ButtonPressed(int buttonID)
    {
        if (gameCompleted) return;

        Debug.Log("Boton " + buttonID + " presionado. Esperado: " + correctSequence[currentStep]);
        
        if (buttonID == correctSequence[currentStep])
        {
            // Correcto! Encender el foco correspondiente
            bulbs[currentStep].TurnOn();
            currentStep++;
            
            Debug.Log("Correcto! Progreso: " + currentStep + "/5");
            
            // Verificar si completó la secuencia
            if (currentStep >= correctSequence.Count)
            {
                Debug.Log("VICTORIA! Secuencia completada!");
                gameCompleted = true;
                Invoke("ReturnToRoom", 1.5f);
            }
        }
        else
        {
            // Incorrecto! Reiniciar
            Debug.Log("Incorrecto! Reiniciando...");
            ResetSequence();
        }
    }

    void ResetSequence()
    {
        currentStep = 0;
        
        // Apagar todos los focos
        foreach (LightBulb bulb in bulbs)
        {
            bulb.TurnOff();
        }
    }

    void ReturnToRoom()
    {
        // Añadir moneda descifrada
        Inventory.AddDecryptedCoin();
        
        SceneManager.LoadScene("Room");
    }
}
