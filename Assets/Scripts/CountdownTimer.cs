using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountdownTimer : Timer
{
    public bool CDTimerDestroyed;
    protected override void EndTimer()
    {
        Actions.OnCountdownEnd?.Invoke();
        displayText.text = ("START!");
        //Destroy(gameObject, 0.5f);
        Invoke("EraseTimer", 0.5f);
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
