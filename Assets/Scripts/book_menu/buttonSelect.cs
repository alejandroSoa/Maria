using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class buttonSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
 public GameObject itemToShow;
 public TMP_Text textMeshPro;
 private bool isClicked = false;
 private static buttonSelect currentlySelected = null;

    public SoundManagerTitle sound;

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
        if (isClicked)
        {
            DeselectButton();
            return;
        }

        if (currentlySelected != null && currentlySelected != this)
        {
            currentlySelected.DeselectButton();
        }

        sound.PlaySelect();
        SelectButton();
    }

    private void SelectButton()
    {
        isClicked = true;
        currentlySelected = this;

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
