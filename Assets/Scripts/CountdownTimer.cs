using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountdownTimer : Timer
{

    protected override void EndTimer()
    {
        Actions.OnCountdownEnd?.Invoke();
        displayText.text = ("START!");
        //Destroy(gameObject, 0.5f);
        Invoke("EraseTimer", 0.5f);
    }

    private void EraseTimer()
    {
        gameObject.SetActive(false);
    }

    private void ResetTimer()
    {
        countdownLength = 3f;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        Actions.OnNextRound += ResetTimer;
    }

    private void OnDisable()
    {
        
    }

}
