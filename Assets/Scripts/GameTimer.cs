using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : Timer
{
    public Text timeUpText;
    //public GameObject gameEndMenu
    public GameObject Player1;
    public GameObject Player2;
    public CountdownTimer countdownTimer;
    public PlayerHealthUIManager p1;
    public PlayerHealthUIManager p2;
    public Movement player1;
    public Movement player2;
    public GameController gameController;
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
        p1.HealthCompare();
        p2.HealthCompare();
       FindObjectOfType<GameController>().SetGameState(GameController.GameStates.nextRoundSetup);
    }


    private void StartGameTimer()
    {
        gameObject.SetActive(true);
        runTimer = true;
        timeUp = false;
    }

    private void ResetGameTimer()
    {
        fix = true;
        Player1.SetActive(true);
        Player2.SetActive(true);
        p1.hurtParticles.Stop();
        p1.deathParticles.Stop();
        p1.blockParticles.Stop();
        p2.hurtParticles.Stop();
        p2.deathParticles.Stop();
        p2.blockParticles.Stop();
        countdownLength = 100;
        displayText.text = ("100");
        runTimer = false;
    }

    //private void StopGameTimer()
    //{
    //    runTimer = false;
    //    print("Stopped");
    //}

    private void OnEnable()
    {
        Actions.OnCountdownEnd += StartGameTimer;
        Actions.OnNextRound += ResetGameTimer;
       // Actions.OnGameOver += StopGameTimer;
    }

    private void OnDisable()
    {
        Actions.OnCountdownEnd -= StartGameTimer;
        Actions.OnNextRound -= ResetGameTimer;
       // Actions.OnGameOver -= StopGameTimer;
    }
}
