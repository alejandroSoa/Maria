using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem; // <<< IMPORTANTE

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public RectTransform nameText;
    public RectTransform nameLeftAnchor;
    public RectTransform nameRightAnchor;
    private DialogueController dialogueController;

    public GameObject dialoguePanel;
    public Image portraitLeft;
    public Image portraitRight;
    public Image portraitNarrador;
    public Image mariaIdle;
    public Image mariaTalking;

    public void ShowDialogue(Dialogue dialogue, string playerName)
    {
        // 1. Mostrar contenido del diálogo
        dialogueText.text = dialogue.Content;

        if (playerName == "Narrador")
        {
            // Ocultar nombre
            nameText.gameObject.SetActive(false);
            portraitNarrador.gameObject.SetActive(true);
            portraitLeft.gameObject.SetActive(false);
            portraitRight.gameObject.SetActive(false);
            mariaIdle.gameObject.SetActive(false);
            mariaTalking.gameObject.SetActive(false);
            return;
        } else if (playerName == "Maria")
        {
            mariaIdle.gameObject.SetActive(false);
            mariaTalking.gameObject.SetActive(true);
        } else if (playerName == "Jugador")
        {
            mariaIdle.gameObject.SetActive(true);
            mariaTalking.gameObject.SetActive(false);
        }

            // Si NO es narrador, aseguramos que el nombre esté visible
            nameText.gameObject.SetActive(true);
        nameText.GetComponent<TextMeshProUGUI>().text = playerName;

        // 2. Activar retratos según side
        portraitNarrador.gameObject.SetActive(false);
        portraitLeft.gameObject.SetActive(dialogue.Side == "left");
        portraitRight.gameObject.SetActive(dialogue.Side == "right");

        // 3. Mover el nombre a su ancla correcta
        if (dialogue.Side == "left")
        {
            nameText.SetParent(nameLeftAnchor);
        }
        else
        {
            nameText.SetParent(nameRightAnchor);
        }

        // 4. Resetear offset dentro del ancla para que quede bien colocado
        nameText.anchoredPosition = Vector2.zero;
    }

    public void SetController(DialogueController controller)
    {
        this.dialogueController = controller;
    }

    void Update()
    {
        // --- NUEVO INPUT SYSTEM ---
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            dialogueController.ShowNextDialogue();
        }
    }

    public void HideUI()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowUI()
    {
        dialoguePanel.SetActive(true);
    }
}
