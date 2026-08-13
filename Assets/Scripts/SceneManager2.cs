using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneManager2 : MonoBehaviour
{
    public int currentScene;
    public PlayerHealthManager ui;
    public PlayerHealthManager ui2;
    public GameEndMenuManager gameEndMenuManager;
    public AudioManager audioManager;

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        CheckSSPress();
    }

    public void OnMouseDown()
    {
        if (gameObject.tag == "PA")
        {
            audioManager.PlaySFX(audioManager.beep);
            ui.losses = 0;
            ui2.losses = 0;
            FindObjectOfType<GameController>().SetGameState(GameController.GameStates.nextRoundSetup);
            gameEndMenuManager.resume.SetActive(false);
            gameEndMenuManager.extraMessage.text = "Preparing the ring...";
        }
    }

    public void MainMenuPressed()
    {
        audioManager.ResumeAudio();
        audioManager.PlaySFX(audioManager.back);
        SceneManager.LoadScene(0);
    }

    public void CheckSSPress()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentScene == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            audioManager.PlaySFX(audioManager.beep);
            SceneManager.LoadScene(1);
        }
    }

    public void CreditPressed()
    {
        audioManager.PlaySFX(audioManager.beep);
        SceneManager.LoadScene(3);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}