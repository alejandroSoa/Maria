using UnityEngine;

public class SoundManagerRoom : MonoBehaviour
{
    public AudioSource changePage, coinObtained, collectObject, errorSound, exitInterface, fusibleIn, fusibleOut, fusibleBoxConfirmation, fusibleBoxExplotion, gateClosed, gateOpen, openInterface, purchase, purchaseError, select, success, turnOff, turnOn, mariaVoice, selbstVoice, unknownVoice, explicationSong, minigamesSong, sqlConsoleSong;

    public void PlayChangePage()
    {
        changePage.Play();
    }

    public void PlayCoinObtained()
    {
        coinObtained.Play();
    }

    public void PlayCollectObject()
    {
        collectObject.Play();
    }

    public void PlayErrorSound()
    {
        errorSound.Play();
    }

    public void PlayExitInterface()
    {
        exitInterface.Play();
    }

    public void PlayFusibleIn()
    {
        fusibleIn.Play();
    }

    public void PlayFusibleOut()
    {
        fusibleOut.Play();
    }

    public void PlayFusibleBoxConfirmation()
    {
        fusibleBoxConfirmation.Play();
    }

    public void PlayFusibleBoxExplotion()
    {
        fusibleBoxExplotion.Play();
    }

    public void PlayGateClosed()
    {
        gateClosed.Play();
    }

    public void PlayGateOpen()
    {
        gateOpen.Play();
    }

    public void PlayOpenInterface()
    {
        openInterface.Play();
    }

    public void PlayPurchase()
    {
        purchase.Play();
    }

    public void PlayPurchaseError()
    {
        purchaseError.Play();
    }

    public void PlaySelect()
    {
        select.Play();
    }

    public void PlaySuccess()
    {
        success.Play();
    }

    public void PlayTurnOff()
    {
        turnOff.Play();
    }

    public void PlayTurnOn()
    {
        turnOn.Play();
    }

    public void PlayMariaVoice()
    {
        mariaVoice.Play();
    }

    public void PlaySelbstVoice()
    {
        selbstVoice.Play();
    }

    public void PlayUnknownVoice()
    {
        unknownVoice.Play();
    }

    public void PlayExplicationSong()
    {
        explicationSong.Play();
    }

    public void PlayMinigamesSong()
    {
        minigamesSong.Play();
    }

    public void PlaySQLConsoleSong()
    {
        sqlConsoleSong.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
