using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ClickableSwapper : MonoBehaviour
{
    [SerializeField] private GameObject targetObjectToShow;
    [SerializeField] private Camera mainCamera;
    
    private void Start()
    {
        InitializeClickableSwapper();
    }
    
    private void Update()
    {
        DetectClicks();
    }
    
    private void InitializeClickableSwapper()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        SetupCollider();
        SetupRigidbody();
    }
    
    private void SetupCollider()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
    
    private void SetupRigidbody()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.simulated = true;
    }

    private void DetectClicks()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject) HandleClick();
        }
    }
    
    private void HandleClick()
    {
        if (targetObjectToShow == null) return;
        targetObjectToShow.SetActive(true);
        gameObject.SetActive(false);
    }

    #region Métodos Públicos
    
    public void ExecuteSwapManually() => HandleClick();
    
    public void SetTargetObject(GameObject newTarget) => targetObjectToShow = newTarget;

    public void SetupSwapChain(GameObject[] objects)
    {
        if (objects == null || objects.Length < 2) return;
        for (int i = 0; i < objects.Length; i++)
        {
            ClickableSwapper swapper = objects[i].GetComponent<ClickableSwapper>() ?? objects[i].AddComponent<ClickableSwapper>();
            swapper.targetObjectToShow = objects[(i + 1) % objects.Length];
        }
    }
    
    public static void SetupBidirectionalSwap(GameObject objA, GameObject objB)
    {
        (objA.GetComponent<ClickableSwapper>() ?? objA.AddComponent<ClickableSwapper>()).targetObjectToShow = objB;
        (objB.GetComponent<ClickableSwapper>() ?? objB.AddComponent<ClickableSwapper>()).targetObjectToShow = objA;
        objB.SetActive(false);
    }
    
    #endregion
    
    private void OnMouseDown() => HandleClick();
}