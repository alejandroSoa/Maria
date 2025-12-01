using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float duration = 2f;

    private float timer = 0f;

    private void Start()
    {
        // Si no viene de una llamada, definimos la escena por default
        if (string.IsNullOrEmpty(SceneLoader.nextScene))
        {
            SceneLoader.nextScene = "Title"; // ← escena por default
        }
    }

    void Update()
    {
        // --- Parpadeo del texto ---
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * 3f));
        loadingText.alpha = alpha;

        // --- Temporizador ---
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            SceneManager.LoadScene(SceneLoader.nextScene);
        }
    }
}
