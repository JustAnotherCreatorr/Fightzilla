using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneManager2 : MonoBehaviour
{
    public int currentScene;
    public PlayerHealthUIManager ui;
    public PlayerHealthUIManager ui2;
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
            ui.resume.SetActive(false);
            ui2.resume.SetActive(false);
            ui.extraMessage.text = "Preparing the ring...";
        }
    }

    public void MainMenuPressed()
    {
        audioManager.PlaySFX(audioManager.beep);
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
        SceneManager.LoadScene(2);
    }

    public void Back()
    {
        audioManager.PlaySFX(audioManager.beep);
        SceneManager.LoadScene(0);
    }

}
