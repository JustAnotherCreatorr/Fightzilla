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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        confirmedP1 = false;
        confirmedP2 = false;
    }

    public void ConfirmPlayer1(GameObject character)
    {
        selectedCharacter = character;
        confirmedP1 = true;
    }

    public void ConfirmPlayer2(GameObject character)
    {
        selectedCharacter2 = character;
        confirmedP2 = true;
    }
}
