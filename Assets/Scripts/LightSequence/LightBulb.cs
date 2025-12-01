using UnityEngine;

public class LightBulb : MonoBehaviour
{
    public SpriteRenderer bulbSprite;
    public Color offColor = Color.red;
    public Color onColor = Color.green;
    
    private bool isOn = false;

    void Start()
    {
        if (bulbSprite == null)
        {
            bulbSprite = GetComponent<SpriteRenderer>();
        }
        TurnOff();
    }

    public void Toggle()
    {
        if (isOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        isOn = true;
        bulbSprite.color = onColor;
    }

    public void TurnOff()
    {
        isOn = false;
        bulbSprite.color = offColor;
    }

    public bool IsOn()
    {
        return isOn;
    }
}
