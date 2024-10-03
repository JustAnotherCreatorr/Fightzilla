using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : Timer
{
    public Text timeUpText;
    //public GameObject gameEndMenu;
    public GameObject Player1;
    public GameObject Player2;
    public CountdownTimer countdownTimer;
    public bool timeUp;

    private void Start()
    {
        timeUpText.gameObject.SetActive(false);
      //  gameEndMenu.gameObject.SetActive(false);
    }


    protected override void EndTimer()
    {

        timeUpText.gameObject.SetActive(true);
        //  gameEndMenu.gameObject.SetActive(true);
        Player1.SetActive(false);
        Player2.SetActive(false);
        timeUp = true;
       // FindObjectOfType<GameController>().SetGameState(GameController.GameStates.nextRoundSetup);
    }


    private void StartGameTimer()
    {
        runTimer = true;
        timeUp = false;
    }

    private void ResetGameTimer()
    {
        Player1.SetActive(true);
        Player2.SetActive(true);
        countdownLength = 100;
        displayText.text = ("100");
        runTimer = false;
    }

    private void OnEnable()
    {
        Actions.OnCountdownEnd += StartGameTimer;
        Actions.OnNextRound += ResetGameTimer;
    }

    private void OnDisable()
    {
        Actions.OnCountdownEnd -= StartGameTimer;
        Actions.OnNextRound -= ResetGameTimer;
    }
}
