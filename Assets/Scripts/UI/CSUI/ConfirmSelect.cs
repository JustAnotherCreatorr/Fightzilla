using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmSelect : MonoBehaviour
{
    public static ConfirmSelect Instance { get; private set; }

    public bool confirmedP1;
    public bool confirmedP2;
    public GameObject selectedCharacter;
    public GameObject selectedCharacter2;

    public GameObject button1;
    public GameObject button2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        confirmedP1 = false;
        confirmedP2 = false;

        if (button1 != null) button1.SetActive(false);
        if (button2 != null) button2.SetActive(false);
    }

    public void ConfirmPlayer1(GameObject character)
    {
        selectedCharacter = character;
        confirmedP1 = true;
        if (button1 != null) button1.SetActive(true);
    }

    public void ConfirmPlayer2(GameObject character)
    {
        selectedCharacter2 = character;
        confirmedP2 = true;
        if (button2 != null) button2.SetActive(true);
    }
}
