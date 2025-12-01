using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

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
