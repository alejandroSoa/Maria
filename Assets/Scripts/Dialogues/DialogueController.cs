// DialogueController.cs
using UnityEngine;
using UnityEngine.InputSystem; // <<< IMPORTANTE
using System.Collections.Generic;

public class DialogueController : MonoBehaviour
{
    public DialogueUI dialogueUI;

    private List<Dialogue> currentDialogues;
    private int index = 0;

    // CONTROLA SI EL DIÁLOGO PUEDE AVANZAR
    public bool continueShowingDialogues = true;

    void Start()
    {
        dialogueUI.SetController(this);
    }

    void Update()
    {
        // --- NUEVO INPUT SYSTEM ---
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            PauseDialogue();
            Debug.Log("Diálogo PAUSADO");
        }

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            ResumeDialogue();
            Debug.Log("Diálogo REANUDADO");
        }
    }

    public void StartDialogueForLevel(int levelId)
    {
        var conn = DatabaseService.Instance.Connection;
        currentDialogues = conn.Table<Dialogue>()
                               .Where(d => d.LevelId == levelId)
                               .OrderBy(d => d.OrderIndex)
                               .ToList();

        index = 0;
        TryShowNextDialogue();
    }

    // Este es el método que se llama al hacer clic
    public void ShowNextDialogue()
    {
        // Si los diálogos están pausados no avanza
        if (!continueShowingDialogues)
            return;

        TryShowNextDialogue();
    }

    private void TryShowNextDialogue()
    {
        // No hay más diálogos
        if (currentDialogues == null || index >= currentDialogues.Count)
        {
            Debug.Log("Fin del diálogo");
            dialogueUI.HideUI();
            return;
        }

        var d = currentDialogues[index];
        var p = DatabaseService.Instance.Connection.Find<Player>(d.PlayerId);

        dialogueUI.ShowDialogue(d, p.Name);

        index++; // SE AVANZA SOLO SI FUE MOSTRADO
    }

    // Método para pausar el diálogo en cualquier momento
    public void PauseDialogue()
    {
        continueShowingDialogues = false;
        dialogueUI.HideUI();
    }

    // Método para continuar exactamente donde se quedó
    public void ResumeDialogue()
    {
        continueShowingDialogues = true;
        dialogueUI.ShowUI();
        TryShowNextDialogue();
    }
}
