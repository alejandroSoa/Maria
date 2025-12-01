using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cables : MonoBehaviour
{
    public SpriteRenderer finalCable;
    public GameObject luz;

    private Vector2 posicionOriginal;
    private Vector2 tamanoOriginal;
    private TareaCables tareaCables;

    void Start()
    {
        posicionOriginal = transform.position;
        tamanoOriginal = finalCable.size;
        
        // Buscar TareaCables en la escena
        tareaCables = FindObjectOfType<TareaCables>();
        
        if (tareaCables == null)
        {
            Debug.LogWarning("No se encontró TareaCables en la escena. Asegúrate de tener un GameObject con el script TareaCables.");
        }
    }

    private bool isDragging = false;

    void Update()
    {
        if (Mouse.current == null) return;

        // Detectar inicio del drag
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                Debug.Log("Empezando a arrastrar: " + gameObject.name);
            }
        }

        // Mientras se arrastra
        if (isDragging && Mouse.current.leftButton.isPressed)
        {
            ActualizarPosicion();
            ComprobarConexion();
            ActualizarRotacion();
            ActualizarTamano();
        }

        // Al soltar
        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            Reiniciar();
        }
    }

    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag detectado en: " + gameObject.name);
        ActualizarPosicion();
        ComprobarConexion();
        ActualizarRotacion();
        ActualizarTamano();
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown detectado en: " + gameObject.name);
    }

    private void ActualizarPosicion()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mousePosition;
    }

    private void ActualizarRotacion()
    {
        Vector2 posicionActual = transform.position;
        Vector2 puntoOrigen = transform.parent.position;

        Vector2 direccion = posicionActual - puntoOrigen;

        float angulo = Vector2.SignedAngle(Vector2.right * transform.lossyScale, direccion);

        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }

    private void ActualizarTamano()
    {
        Vector2 posicionActual = transform.position;
        Vector2 puntoOrigen = transform.parent.position;

        float distancia = Vector2.Distance(posicionActual, puntoOrigen);

        finalCable.size = new Vector2(distancia, finalCable.size.y);
    }

    private void Reiniciar()
    {
        transform.position = posicionOriginal;
        transform.rotation = Quaternion.identity;
        finalCable.size = tamanoOriginal;
    }

    private void ComprobarConexion()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

        foreach (Collider2D col in colliders)
        {
            // No procesamos el collider del cable que estamos moviendo.
            if (col.gameObject != gameObject)
            {
                transform.position = col.transform.position;

                Cables otroCable = col.gameObject.GetComponent<Cables>();

                if (otroCable != null && finalCable.color == otroCable.finalCable.color)
                {
                    // Conexion correcta.
                    Conectar();
                    otroCable.Conectar();

                    if (tareaCables != null)
                    {
                        tareaCables.conexionesActuales++;
                        tareaCables.ComprobarVictoria();
                    }
                }
            }
        }
    }

    public void Conectar()
    {
        if (luz != null)
        {
            luz.SetActive(true);
        }
        Destroy(this);
    }

}
