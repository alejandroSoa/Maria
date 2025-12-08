using UnityEngine;

/// <summary>
/// Gestiona la activaci�n y desactivaci�n de las diferentes vistas de la habitaci�n.
/// Todas las vistas est�n en la misma escena MainRoom.
/// Solo una vista debe estar activa a la vez.
/// Los botones Left y Right crean un efecto de rotaci�n circular.
/// </summary>

public class viewManager : MonoBehaviour
{
    public static viewManager Instance;
    [Header("Referencias a las Vistas")]
    [Tooltip("Vista frontal de la habitaci�n")]
    public GameObject View_Front;

    [Tooltip("Vista izquierda de la habitaci�n")]
    public GameObject View_Left;

    [Tooltip("Vista derecha de la habitaci�n")]
    public GameObject View_Right;

    [Tooltip("Vista trasera de la habitación")]
    public GameObject View_Back;

    // Enumeración para representar las direcciones de rotación
    private enum ViewDirection
    {
        Front = 0,
        Right = 1,
        Back = 2,
        Left = 3
    }

    // Estado actual de la rotaci�n
    private ViewDirection currentDirection = ViewDirection.Front;

    private void Start()
    {
        // Al iniciar, activar solo la vista frontal por defecto
        ShowFront();

    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    /// <summary>
    /// Muestra la vista frontal y oculta todas las dem�s.
    /// Este m�todo se llama desde el bot�n "Enfrente" en el Canvas.
    /// </summary>
    public void ShowFront()
    {
        currentDirection = ViewDirection.Front;
        ActivateView(View_Front);
    }

    /// <summary>
    /// Rota hacia la izquierda (sentido horario).
    /// Este método se llama desde el botón "Izquierda" en el Canvas.
    /// </summary>
    public void ShowLeft()
    {
        RotateLeft();
    }

    /// <summary>
    /// Rota hacia la derecha (sentido antihorario).
    /// Este m�todo se llama desde el bot�n "Derecha" en el Canvas.
    /// </summary>
    public void ShowRight()
    {
        RotateRight();
    }

    /// <summary>
    /// Muestra la vista opuesta a la actual.
    /// Este método se llama desde el botón "Atras" en el Canvas.
    /// Front <-> Back, Left <-> Right
    /// </summary>
    public void ShowBack()
    {
        // Obtener la vista opuesta sumando 2 a la dirección actual (módulo 4)
        currentDirection = (ViewDirection)(((int)currentDirection + 2) % 4);
        ActivateCurrentView();
    }

    /// <summary>
    /// Rota la vista hacia la izquierda (sentido horario).
    /// Front -> Left -> Back -> Right -> Front
    /// </summary>
    private void RotateLeft()
    {
        currentDirection = (ViewDirection)(((int)currentDirection + 3) % 4);
        ActivateCurrentView();
    }

    /// <summary>
    /// Rota la vista hacia la derecha (sentido antihorario).
    /// Front -> Right -> Back -> Left -> Front
    /// </summary>
    private void RotateRight()
    {
        currentDirection = (ViewDirection)(((int)currentDirection + 1) % 4);
        ActivateCurrentView();
    }

    /// <summary>
    /// Activa la vista correspondiente a la direcci�n actual.
    /// </summary>
    private void ActivateCurrentView()
    {
        switch (currentDirection)
        {
            case ViewDirection.Front:
                ActivateView(View_Front);
                break;
            case ViewDirection.Right:
                ActivateView(View_Right);
                break;
            case ViewDirection.Back:
                ActivateView(View_Back);
                break;
            case ViewDirection.Left:
                ActivateView(View_Left);
                break;
        }
    }

    /// <summary>
    /// Activa la vista especificada y desactiva todas las demás.
    /// </summary>
    /// <param name="viewToActivate">La vista que se debe activar</param>
    public void ActivateView(GameObject viewToActivate)
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

    public void DeactivateView(GameObject viewToDeactivate)
    {
        if (viewToDeactivate != null)
        {
            viewToDeactivate.SetActive(false);
        }
    }
}
