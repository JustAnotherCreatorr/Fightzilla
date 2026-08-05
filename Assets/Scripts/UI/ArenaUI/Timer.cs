using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Timer : MonoBehaviour
{

    [SerializeField] protected Text displayText;

    [SerializeField] protected float countdownLength;

    [SerializeField] protected bool startImmediate;
    [SerializeField] protected bool showDecimals;
    protected bool runTimer;
    public bool fix = true;
    public PauseMenu pm;
    public GameEndMenuManager gemm;


    protected virtual void Start()
    {
        if (startImmediate)
        {
            runTimer = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (pm.paused)
        {
            return;
        }

        if (runTimer == false)
        {
            return;
        }

        if (fix == false)
        {
            return;
        }

        if (countdownLength > 0)
        {
            countdownLength -= Time.deltaTime;
            UpdateTimerText();

            if (countdownLength <= 0)
            {
                countdownLength = 0;
                UpdateTimerText();
                EndTimer();
            }
        }
    }

    protected abstract void EndTimer();

    private void UpdateTimerText()
    {
        if (showDecimals)
        {
            displayText.text = countdownLength.ToString("F1");
        } else
        {
            displayText.text = countdownLength.ToString("F0");
        }
    }
}
