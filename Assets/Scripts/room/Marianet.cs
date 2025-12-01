using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marianet : MonoBehaviour
{
    [SerializeField] private Button buttonBuyInt;
    [SerializeField] private Button buttonBuyDate;
    [SerializeField] private Button buttonBuyVarchar;
    [SerializeField] private Button buttonBuyBool;

    public SoundManagerRoom soundRoom;

    public TextMeshProUGUI koensText;

    int MariaKoens = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonBuyInt.onClick.AddListener(() => OnBuyButtonClicked("INT"));
        buttonBuyDate.onClick.AddListener(() => OnBuyButtonClicked("DATE"));
        buttonBuyVarchar.onClick.AddListener(() => OnBuyButtonClicked("VARCHAR"));
        buttonBuyBool.onClick.AddListener(() => OnBuyButtonClicked("BOOL"));

        UpdateKoensText();
    }

    void OnBuyButtonClicked(string tipo)
    {
        if (MariaKoens <= 0)
        {
            soundRoom.PlayPurchaseError();
            Debug.Log($"No tienes Koens suficientes para comprar {tipo}");
        }
        else
        {
            MariaKoens--;
            soundRoom.PlayPurchase();
            Debug.Log($"Botón {tipo} clickeado");
            UpdateKoensText();
        }
    }

    void UpdateKoensText()
    {
        if (koensText != null)
        {
            koensText.text = MariaKoens.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
