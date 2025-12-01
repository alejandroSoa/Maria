using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public DialogueController controller;

    void Start()
    {
        controller.StartDialogueForLevel(1);
    }
}

