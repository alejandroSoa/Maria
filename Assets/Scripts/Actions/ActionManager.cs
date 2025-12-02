using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    public static bool showMaria = false;
    public static int savedDialogueIndex = -1;
    public static bool playing = false;

    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearActions()
    {
        actions.Clear();
    }

    // Registrar acciones desde cualquier script
    public void RegisterAction(string actionName, Action action)
    {
        if (!actions.ContainsKey(actionName))
        {
            actions.Add(actionName, action);
        }
    }

    // Ejecutar acciones
    public void TriggerAction(string actionName)
    {
        if (actions.ContainsKey(actionName))
        {
            actions[actionName].Invoke();
        }
        else
        {
            Debug.LogWarning("No action found for: " + actionName);
        }
    }
}
