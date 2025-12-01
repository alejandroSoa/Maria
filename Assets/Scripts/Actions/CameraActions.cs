using System.Collections;
using UnityEngine;

public class CameraActions : MonoBehaviour
{
    public Transform targetCajaFusibles;
    public Camera cam;

    // OFFSET para ajustar el zoom hacia arriba/abajo
    public float zoomOffsetY = 25f;

    void Start()
    {
        ActionManager.Instance.RegisterAction("ZOOM_Caja_Fusibles", () =>
        {
            StartCoroutine(ZoomToTarget(targetCajaFusibles, 2f));
        });

        ActionManager.Instance.RegisterAction("RESET_CAMERA", () =>
        {
            cam.orthographicSize = 5;
        });
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
