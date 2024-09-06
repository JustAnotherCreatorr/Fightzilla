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
        Destroy(gameObject, 0.5f);
    }

}
