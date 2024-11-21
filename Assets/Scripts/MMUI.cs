using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMUI : MonoBehaviour
{
    public GameObject mainBox;
    public GameObject p1movementText;
    public GameObject p1combatText;
    public GameObject p2movementText;
    public GameObject p2combatText;
    public GameObject credits;
    public bool alreadyOpen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void OnMouseDown()
    {
        if (gameObject.tag == "p1m")
        {
            p1movementText.SetActive(true);
            p1combatText.SetActive(false);
        }

        if (gameObject.tag == "p1c")
        {
            p1combatText.SetActive(true);
            p1movementText.SetActive(false);
        }

        if (gameObject.tag == "p2m")
        {
            p2movementText.SetActive(true);
            p2combatText.SetActive(false);
        }

        if (gameObject.tag == "p2c")
        {
            p2movementText.SetActive(false);
            p2combatText.SetActive(true);
        }

        if (gameObject.tag == "mainButton")
        {
            if (alreadyOpen)
            {
                mainBox.SetActive(false);
                alreadyOpen = false;
                return;
            }

            mainBox.SetActive(true);
            alreadyOpen = true;
        }
    }
}
