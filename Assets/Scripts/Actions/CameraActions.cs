using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraActions : MonoBehaviour
{
    public Transform targetCajaFusibles;
    public Camera cam;
    public GameObject A1E2;
    public Image fuseBox;
    public Image bathroomTable;
    public Image fuseBoxWithFuses;
    public Image fuseBoxWithNoFuses;
    public GameObject desencriptadoraRoom;
    public GameObject mariaNetRoom;
    public Transform desencriptadora;
    public GameObject interfazMariaNet;
    public GameObject initialRoom;
    public Image maria;

    private Vector3 originalCamPos;
    private float originalCamSize;


    // OFFSET para ajustar el zoom hacia arriba/abajo
    public float zoomOffsetY = 25f;

    void Awake()
    {
        ActionManager.Instance.ClearActions();
    }

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("No se encontr� la c�mara principal.");
                return;
            }
        }
        if (ActionManager.playing)
        {
            viewManager.Instance.DeactivateView(initialRoom);
            viewManager.Instance.ActivateView(desencriptadoraRoom);
        }
        originalCamPos = cam.transform.position;
        originalCamSize = cam.orthographicSize;
        ActionManager.Instance.RegisterAction("Mostrar_Maria", () =>
        {
            ActionManager.showMaria = true;
        });

        ActionManager.Instance.RegisterAction("ZOOM_Caja_Fusibles", () =>
        {
            StartCoroutine(ZoomToTarget(targetCajaFusibles, 2f));
        });

        ActionManager.Instance.RegisterAction("MOSTRAR_CAJA_TABLA", () =>
        {
            A1E2.SetActive(true);
        });

        ActionManager.Instance.RegisterAction("Quitar_Caja_Fusibles", () =>
        {
            fuseBox.gameObject.SetActive(false);
        });

        ActionManager.Instance.RegisterAction("Quitar_Bathroom_Table", () =>
        {
            bathroomTable.gameObject.SetActive(false);
            fuseBoxWithFuses.gameObject.SetActive(true);
            fuseBoxWithNoFuses.gameObject.SetActive(true);
        });

        ActionManager.Instance.RegisterAction("Quitar_A1E2", () =>
        {
            A1E2.SetActive(false);
        });

        ActionManager.Instance.RegisterAction("Mostrar_Cuarto_Desencriptadora", () =>
        {
            cam.transform.position = originalCamPos;
            cam.orthographicSize = originalCamSize;
            viewManager.Instance.ActivateView(desencriptadoraRoom);
        });

        ActionManager.Instance.RegisterAction("Zoom_desencriptadora", () =>
        {
            StartCoroutine(ZoomToTarget(desencriptadora, 1f));
        });

        ActionManager.Instance.RegisterAction("Jugar_Desencriptadora", () =>
        {
            ActionManager.savedDialogueIndex = DialogueController.Instance.GetCurrentIndex();
            ActionManager.playing = true;
            cam.transform.position = originalCamPos;
            cam.orthographicSize = originalCamSize;
            DialogueController.Instance.PauseDialogue();
        });

        ActionManager.Instance.RegisterAction("Volver_desencriptadora_room", () =>
        {
            cam.transform.position = originalCamPos;
            cam.orthographicSize = originalCamSize;
            viewManager.Instance.DeactivateView(initialRoom);
            viewManager.Instance.ActivateView(desencriptadoraRoom);
        });

        ActionManager.Instance.RegisterAction("Cargar_MariaNet_Room", () =>
        {
            viewManager.Instance.DeactivateView(desencriptadoraRoom);
            viewManager.Instance.ActivateView(mariaNetRoom);
        });

        ActionManager.Instance.RegisterAction("Activar_interfaz_marianet", () =>
        {
            interfazMariaNet.SetActive(true);
            DialogueController.Instance.PauseDialogue();
        });

        ActionManager.Instance.RegisterAction("Pausar_dialogo", () =>
        {
            interfazMariaNet.SetActive(false);
            StartCoroutine(ResumeDialogueAfterDelay(2f));
        });

        ActionManager.Instance.RegisterAction("Dejar_jugador_jugar", () =>
        {
            viewManager.Instance.DeactivateView(mariaNetRoom);
            viewManager.Instance.ActivateView(initialRoom);
            DialogueController.Instance.PauseDialogue();
        });
    }

    private IEnumerator ResumeDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DialogueController.Instance.PauseDialogue();
    }

    private IEnumerator ZoomToTarget(Transform target, float targetSize)
    {
        if (target == null)
        {
            Debug.LogError("ZoomToTarget: TARGET ES NULL. Asigna targetCajaFusibles en el inspector.");
            yield break;
        }

        Vector3 startPos = cam.transform.position;

        Vector3 targetPos = new Vector3(
            target.position.x,
            target.position.y,
            startPos.z
        );

        float startSize = cam.orthographicSize;
        float t = 0f;

        while (t < 1f)
        {
            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }
        cam.transform.position = targetPos;
        cam.orthographicSize = targetSize;
    }
}
