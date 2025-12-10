using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InterfazMando : MonoBehaviour
{
    public int salaActual;
    public bool salaElegida;
    public bool salaEstado;

    public TextMeshProUGUI salaActualText;
    public Button sala1, sala2, sala3, sala4;
    private GameObject x1, x2, x3, x4;

    void Start()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 2)
            salaActual = 1;
        else if (currentScene == 6)
            salaActual = 2;
        else if (currentScene == 7)
            salaActual = 3;
        else if (currentScene == 8)
            salaActual = 4;

        salaElegida = false;

        sala1.onClick.AddListener(() => ElegirSala(1));
        sala2.onClick.AddListener(() => ElegirSala(2));
        sala3.onClick.AddListener(() => ElegirSala(3));
        sala4.onClick.AddListener(() => ElegirSala(4));

        x1 = sala1.transform.Find("X")?.gameObject;
        x2 = sala2.transform.Find("X")?.gameObject;
        x3 = sala3.transform.Find("X")?.gameObject;
        x4 = sala4.transform.Find("X")?.gameObject;

        // Inicializar el texto y las X
        if (salaActualText != null)
            salaActualText.text = salaActual.ToString();

        ActualizarX();
    }

    public void ElegirSala(int numeroSala)
    {
        salaActual = numeroSala;
        salaElegida = true;
        salaActualText.text = salaActual.ToString();
    }

    void ActualizarX()
    {
        // Apagar todas las X
        if (x1 != null) x1.SetActive(false);
        if (x2 != null) x2.SetActive(false);
        if (x3 != null) x3.SetActive(false);
        if (x4 != null) x4.SetActive(false);

        // Activar la X del botón anterior a la sala actual
        if (salaActual == 2 && x1 != null) x1.SetActive(true);
        if (salaActual == 3 && x2 != null) x2.SetActive(true);
        if (salaActual == 4 && x3 != null) x3.SetActive(true);
    }

    void Update()
    {

    }
}