using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class prototypingbattlebucks : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{

    public float currentAmount;
    public float newAmount;

    public Text Amount;

    public Button winCPU;
    public Button winP2;
    public Button winCPUTT;
    public Button winP2TT;
    public Button noWinTM;
    public Button ONEwinTM;
    public Button TWOwinTM;
    public Button winTM;

    public bool buttonPressed;

    public buttonValue value;

    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }


    void Update()
    {
        if (buttonPressed)
        {
            currentAmount += value.value = newAmount;
            Amount.text = newAmount.ToString();
        }
    }

}
