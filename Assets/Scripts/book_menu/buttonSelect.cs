using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class buttonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
 public GameObject itemToShow;
 public TMP_Text textMeshPro;
 private bool isClicked = false;
 private static buttonSelect currentlySelected = null; 

    void Start()
    {
        if (itemToShow != null)
        {
            itemToShow.SetActive(false);
        }

        if (textMeshPro != null)
        {
            textMeshPro.fontStyle &= ~FontStyles.Underline;
        }
    }

    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Only show hover effects if button hasn't been clicked
        if (!isClicked)
        {
            if (itemToShow != null)
            {
                itemToShow.SetActive(true);
            }

            if (textMeshPro != null)
            {
                textMeshPro.fontStyle |= FontStyles.Underline; 
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Only hide hover effects if button hasn't been clicked
        if (!isClicked)
        {
            if (itemToShow != null)
            {
                itemToShow.SetActive(false);
            }

            if (textMeshPro != null)
            {
                textMeshPro.fontStyle &= ~FontStyles.Underline;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If this button is already selected, deselect it
        if (isClicked)
        {
            DeselectButton();
            return;
        }

        // If there's another button selected, deselect it first
        if (currentlySelected != null && currentlySelected != this)
        {
            currentlySelected.DeselectButton();
        }

        // Select this button
        SelectButton();
    }

    private void SelectButton()
    {
        isClicked = true;
        currentlySelected = this;

        // Show item and underline
        if (itemToShow != null)
        {
            itemToShow.SetActive(true);
        }

        if (textMeshPro != null)
        {
            textMeshPro.fontStyle |= FontStyles.Underline;
        }
    }

    private void DeselectButton()
    {
        isClicked = false;
        
        if (currentlySelected == this)
        {
            currentlySelected = null;
        }

        // Hide item and remove underline
        if (itemToShow != null)
        {
            itemToShow.SetActive(false);
        }

        if (textMeshPro != null)
        {
            textMeshPro.fontStyle &= ~FontStyles.Underline;
        }
    }
}
