using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountdownTimer : Timer
{
    public bool CDTimerDestroyed;
    public AudioManager audioManager;

    protected override void Start()
    {
        base.Start();
        audioManager.PlaySFX(audioManager.countdown);
    }

    protected override void EndTimer()
    {
        Actions.OnCountdownEnd?.Invoke();
        displayText.text = ("START!");
        Invoke("EraseTimer", 0.5f);
        FindObjectOfType<GameController>().SetGameState(GameController.GameStates.inPlay);
    }

    private void EraseTimer()
    {
        displayText.text = string.Empty;
        CDTimerDestroyed = true;
    }

    private void ResetTimer()
    {
        countdownLength = 3f;
        CDTimerDestroyed = false;
        audioManager.PlaySFX(audioManager.countdown);
    }

    private void OnEnable()
    {
        Actions.OnNextRound += ResetTimer;
    }

    private void OnDisable()
    {
       Actions.OnNextRound -= ResetTimer;
    }

}
