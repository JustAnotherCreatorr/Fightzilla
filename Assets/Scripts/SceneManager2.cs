using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager2 : MonoBehaviour
{
    public int currentScene;
    public PlayerHealthUIManager ui;
    public PlayerHealthUIManager ui2;

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
            ui.losses = 0;
            ui2.losses = 0;
            FindObjectOfType<GameController>().SetGameState(GameController.GameStates.nextRoundSetup);
            ui.extraMessage.text = "Preparing the ring...";
        }
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void CheckSSPress()
    {
        if (Input.anyKey && currentScene == 1)
        {
            SceneManager.LoadScene(0);
        }
    }

}
