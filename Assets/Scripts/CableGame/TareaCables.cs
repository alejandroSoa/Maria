using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TareaCables : MonoBehaviour
{
    public int conexionesActuales;

    public void ComprobarVictoria()
    {
        if (conexionesActuales == 6)
        {
            Debug.Log("¡Victoria! Regresando a Room...");
            Invoke("VolverARoom", 1f);
        }
    }

    void VolverARoom()
    {
        // Añadir moneda descifrada
        Inventory.AddDecryptedCoin();
        
        SceneManager.LoadScene("Room");
    }
}
