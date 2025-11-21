using UnityEngine;

public class level_state : MonoBehaviour
{
    [Header("Level State GameObjects")]
    public GameObject done;
    public GameObject locked;
    public GameObject available;
    
    [Header("Level Configuration")]
    public string levelPrefsKey; 
    
    void Start()
    {
        SetLevelState();
    }

    void Update()
    {
        
    }
    
    void SetLevelState()
    {
        done.SetActive(false);
        locked.SetActive(false);
        available.SetActive(false);
        
        switch (PlayerPrefs.GetString(levelPrefsKey))
        {
            case "done":
                done.SetActive(true);
                break;
            case "locked":
                locked.SetActive(true);
                break;
            case "available":
            default:
                available.SetActive(true);
                break;
        }
    }
    
    public void SetLevelDone()
    {
        PlayerPrefs.SetString(levelPrefsKey, "done");
        PlayerPrefs.Save();
        SetLevelState();
    }
    
    public void SetLevelLocked()
    {
        PlayerPrefs.SetString(levelPrefsKey, "locked");
        PlayerPrefs.Save();
        SetLevelState();
    }
    
    public void SetLevelAvailable()
    {
        PlayerPrefs.SetString(levelPrefsKey, "available");
        PlayerPrefs.Save();
        SetLevelState();
    }
    
    public void OnClickLevel()
    {
        PlayerPrefs.SetString("selectedlevel", levelPrefsKey);
        PlayerPrefs.Save();
    }
}
