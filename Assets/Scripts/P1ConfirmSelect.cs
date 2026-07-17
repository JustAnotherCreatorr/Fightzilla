using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1ConfirmSelect : MonoBehaviour
{
    public static P1ConfirmSelect Instance { get; private set; }
    public bool confirmedP1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCurrentPlayerSelecting(HoverPanel hoverPanel)
    {
        Debug.Log("ccps");
        if (hoverPanel.currentDisplay == hoverPanel.display)
        {
            Debug.Log("calledIF");
            confirmedP1 = true;
        }
    }
}
