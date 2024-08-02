using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class prototypingbattlebucks : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{

    //public float currentAmount;
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

    public Button selectedButton;

    public bool buttonPressed;

    public float value;
   
    // Start is called before the first frame update

    void Start()
    {
        newAmount = PlayerPrefs.GetFloat("Amount");
        Amount.text = newAmount.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        Debug.Log("pointer is down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        Debug.Log("pointer is up");
    }


    void Update()
    {
        
    }

    public void OnMouseDown()
    {
            Debug.Log("buttonPressed");
            newAmount = PlayerPrefs.GetFloat("Amount") + value;
            PlayerPrefs.SetFloat("Amount", newAmount);
            Amount.text = newAmount.ToString();
    }

}
