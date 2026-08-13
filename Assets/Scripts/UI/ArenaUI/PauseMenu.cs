using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool paused;
    public CountdownTimer CT;
    public bool gameDone;

    public GameObject GEM;
    public GameObject GEMbox;
    public GameObject PA;
    public GameObject resume;
    public Text whichPlayerWin;
    public Text extraMessage;
    public Outline GEMplayerWon;
    public Color blank;
    public GameObject mainMenuButton;
    public GameObject MMtext;
    public Color normalButtonColor;

    public AudioManager audioManager;
    public Movement playerMovement;
    public Movement otherPlayerMovement;

    public GameEndMenuManager gameEndMenuManager;

    void Update()
    {
        bool spacePressed = Input.GetKeyDown(KeyCode.P);
        Paused(spacePressed);
    }

    public void Paused(bool spacePressed)
    {
        if (CT.end == false)
        {
            return;
        }

        if (gameEndMenuManager.GEMActive)
        {
            return;
        }
    
        if (!spacePressed)
        {
            return;
        }

        if (gameDone)
        {
            return;
        }

        if (gameEndMenuManager.settingUpNR)
        {
            return;
        }

        audioManager.PlaySFX(audioManager.beep);
        playerMovement.allowMovement = false;
        otherPlayerMovement.allowMovement = false;
        whichPlayerWin.color = blank;
        GEMplayerWon.effectColor = blank;
        paused = true;
        audioManager.PauseAudio();
        GEM.SetActive(true);
        PA.SetActive(false);
        GEMbox.GetComponent<Image>().color = Color.black;
        mainMenuButton.GetComponent<Image>().color = normalButtonColor;
        mainMenuButton.GetComponent<Outline>().effectColor = Color.white;
        MMtext.GetComponent<Text>().color = Color.white;
        whichPlayerWin.text = "Paused";
        extraMessage.text = "The fight never ends!";
        resume.SetActive(true);
    }

    public void Resume()
    {
        playerMovement.allowMovement = true;
        otherPlayerMovement.allowMovement = true;
        paused = false;
        audioManager.ResumeAudio();
        audioManager.PlaySFX(audioManager.back);
        GEM.SetActive(false);
        resume.SetActive(false);
    }
}
