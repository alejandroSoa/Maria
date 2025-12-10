using UnityEngine;

public class SoundManagerTitle : MonoBehaviour
{
    public AudioSource Select;

    public void PlaySelect()
    {
        Select.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("stopDialogues", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
