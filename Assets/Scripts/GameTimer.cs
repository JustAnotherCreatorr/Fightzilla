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

    private void Start()
    {
        timeUpText.gameObject.SetActive(false);
      //  gameEndMenu.gameObject.SetActive(false);
    }


    protected override void EndTimer()
    {

        timeUpText.gameObject.SetActive(true);
      //  gameEndMenu.gameObject.SetActive(true);
        Destroy(Player1);
        Destroy(Player2);
    }


    private void StartGameTimer()
    {
        runTimer = true;
    }

    private void ResetGameTimer()
    {
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
        //Actions.OnNextRound -= ResetGameTimer;
    }
}
