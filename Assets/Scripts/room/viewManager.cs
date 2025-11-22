using UnityEngine;

/// <summary>
/// Gestiona la activación y desactivación de las diferentes vistas de la habitación.
/// Todas las vistas están en la misma escena MainRoom.
/// Solo una vista debe estar activa a la vez.
/// </summary>

public class viewManager : MonoBehaviour
{
    [Header("Referencias a las Vistas")]
    [Tooltip("Vista frontal de la habitación")]
    public GameObject View_Front;

    [Tooltip("Vista izquierda de la habitación")]
    public GameObject View_Left;

    [Tooltip("Vista derecha de la habitación")]
    public GameObject View_Right;

    [Tooltip("Vista trasera de la habitación")]
    public GameObject View_Back;

    private void Start()
    {
        // Al iniciar, activar solo la vista frontal por defecto
        ShowFront();
    }

    /// <summary>
    /// Muestra la vista frontal y oculta todas las demás.
    /// Este método se llama desde el botón "Enfrente" en el Canvas.
    /// </summary>
    public void ShowFront()
    {
        ActivateView(View_Front);
    }

    /// <summary>
    /// Muestra la vista izquierda y oculta todas las demás.
    /// Este método se llama desde el botón "Izquierda" en el Canvas.
    /// </summary>
    public void ShowLeft()
    {
        ActivateView(View_Left);
    }

    /// <summary>
    /// Muestra la vista derecha y oculta todas las demás.
    /// Este método se llama desde el botón "Drecha" en el Canvas.
    /// </summary>
    public void ShowRight()
    {
        ActivateView(View_Right);
    }

    /// <summary>
    /// Muestra la vista trasera y oculta todas las demás.
    /// Este método se llama desde el botón "Atras" en el Canvas.
    /// </summary>
    public void ShowBack()
    {
        ActivateView(View_Back);
    }

    /// <summary>
    /// Activa la vista especificada y desactiva todas las demás.
    /// </summary>
    /// <param name="viewToActivate">La vista que se debe activar</param>
    private void ActivateView(GameObject viewToActivate)
    {
        // Desactivar todas las vistas
        if (View_Front != null) View_Front.SetActive(false);
        if (View_Left != null) View_Left.SetActive(false);
        if (View_Right != null) View_Right.SetActive(false);
        if (View_Back != null) View_Back.SetActive(false);

        // Activar solo la vista seleccionada
        if (viewToActivate != null)
        {
            viewToActivate.SetActive(true);
        }
        else
        {
            Debug.LogWarning("ViewManager: La vista a activar es null. Verifica las referencias en el Inspector.");
        }
    }
}
