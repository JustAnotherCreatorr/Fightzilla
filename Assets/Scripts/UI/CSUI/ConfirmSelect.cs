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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCurrentPlayerSelecting(HoverPanel hoverPanel)
    {
        if (hoverPanel.currentDisplay == hoverPanel.otherDisplay)
        {
            confirmedP2 = true;
        }

        if (hoverPanel.currentDisplay == hoverPanel.display)
        {
            confirmedP1 = true;
        }
    }
}
