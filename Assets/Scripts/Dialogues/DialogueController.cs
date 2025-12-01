// DialogueController.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem; // <<< IMPORTANTE
using static UnityEngine.EventSystems.EventTrigger;

public class DialogueController : MonoBehaviour
{
    public DialogueUI dialogueUI;

    private List<Dialogue> currentDialogues;
    private int index = 0;

    // CONTROLA SI EL DIÁLOGO PUEDE AVANZAR
    public bool continueShowingDialogues = true;

    public static DialogueController Instance;
    private bool isResuming = false;


    private Dictionary<string, string> specialActions = new Dictionary<string, string>
{
    { "Un rostro feliz aparece en la pantalla del dispositivo.", "Mostrar_Maria" },
    { "Se acercan a la caja de fusibles en la cual van a poder empezar a trabajar con los problemas de base de datos.", "ZOOM_Caja_Fusibles" },
    { "Maria muestra una caja de fusibles, esta tiene una apariencia similar a una tabla, la tabla tiene como nombre ‘BathroomCurtains’ e incluye campos dentro de la tabla.", "MOSTRAR_CAJA_TABLA" },
    { "Si tienes duda sobre cortinas de baño también te puedo ayudar, pero, concentrémonos en salir primero.", "Quitar_Caja_Fusibles" },
    { "Entonces, solo necesitamos volver a conectarle fusibles funcionales a esta caja, columna, a esta a cosa, y con eso la energía volverá y yo podré ir a casa, ¿verdad?", "Quitar_Bathroom_Table" },
    { "Ahora, necesitamos resolver la siguiente tabla, aprovechemos lo que sabemos y empecemos con ello.", "Quitar_A1E2" },
    { "¿Cómo hacemos que esto…?", "Mostrar_Cuarto_Desencriptadora" },
    { "Dentro de la desencriptadora, podrás solicitar ciertos recursos, pero, requieren cierta maña.", "Zoom_desencriptadora" },
    { "¡Exacto! Comienza, para que puedas obtener el fusible correcto.", "Jugar_Desencriptadora"},
    { "Ya lo hice, admito que tuvo lo suyo, pero, esto no es un fusible.", "Volver_desencriptadora_room" },
    { "¡Bienvenido a MariaNet!", "Cargar_MariaNet_Room" },
    { "Ahora sí, selecciona el fusible de tipo INT por favor.", "Activar_interfaz_marianet" },
    { "Toma el fusible de la esclusa de ahí.", "Quitar_interfaz_marianet" },
    { "¡Revisa la caja de fusibles para ver qué tipo de fusible necesitas!", "Dejar_jugador_jugar" }

};

    void Awake()
    {
        Instance = this;
    }

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

        CheckForSpecialDialogue(d);

        index++; // SE AVANZA SOLO SI FUE MOSTRADO
    }

    private void CheckForSpecialDialogue(Dialogue dialogue)
    {
        if (specialActions.ContainsKey(dialogue.Content))
        {
            string actionName = specialActions[dialogue.Content];
            ActionManager.Instance.TriggerAction(actionName);
        }
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
        // Evita reentradas y loops infinitos
        if (isResuming) return;

        isResuming = true;

        continueShowingDialogues = true;
        dialogueUI.ShowUI();

        TryShowNextDialogue();

        isResuming = false;
    }

}
