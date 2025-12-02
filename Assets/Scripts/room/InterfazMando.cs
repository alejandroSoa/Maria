using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfazMando : MonoBehaviour
{
    public int salaActual;
    public bool salaElegida;
    public bool salaEstado;

    public TextMeshProUGUI salaActualText;
    public Button sala1, sala2, sala3, sala4;

    void Start()
    {
        salaActual = 1;
        salaElegida = false;

        sala1.onClick.AddListener(() => ElegirSala(1));
        sala2.onClick.AddListener(() => ElegirSala(2));
        sala3.onClick.AddListener(() => ElegirSala(3));
        sala4.onClick.AddListener(() => ElegirSala(4));

        // Mostrar la sala inicial en el texto
        if (salaActualText != null)
            salaActualText.text = salaActual.ToString();
    }

    public void ElegirSala(int numeroSala)
    {
        salaActual = numeroSala;
        salaElegida = true;
        salaActualText.text = salaActual.ToString();
    }

    void Update()
    {

    }
}