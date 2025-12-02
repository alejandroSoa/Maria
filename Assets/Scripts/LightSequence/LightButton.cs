using UnityEngine;
using UnityEngine.InputSystem;

public class LightButton : MonoBehaviour
{
    public int buttonID; // 0, 1, 2, 3, 4
    public SpriteRenderer buttonSprite;
    public Color normalColor = Color.white;
    public Color pressedColor = Color.yellow;
    
    private LightSequenceManager manager;

    void Start()
    {
        manager = FindFirstObjectByType<LightSequenceManager>();
        
        if (buttonSprite == null)
        {
            buttonSprite = GetComponent<SpriteRenderer>();
        }
        
        buttonSprite.color = normalColor;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                OnClick();
            }
        }
    }

    void OnClick()
    {
        if (manager != null)
        {
            manager.ButtonPressed(buttonID);
            StartCoroutine(PressEffect());
        }
    }

    System.Collections.IEnumerator PressEffect()
    {
        buttonSprite.color = pressedColor;
        yield return new WaitForSeconds(0.15f);
        buttonSprite.color = normalColor;
    }
}
