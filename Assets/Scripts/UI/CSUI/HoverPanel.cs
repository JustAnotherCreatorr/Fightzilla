using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int UInumber;

    public bool confirmedP1;

    public TMP_Text currentDisplay;
    public TMP_Text display;
    public TMP_Text otherDisplay;
    public string characterName;

    public Color characterColor;
    public Color hoverPanelColor;
    public Color defaultPanelColor;

    public GameObject hoverOutline;
    public Outline panelOutline;

    // Start is called before the first frame update
    void Start()
    {
        UInumber = 1;
        currentDisplay = display;
        confirmedP1 = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string colorCode = ColorUtility.ToHtmlStringRGB(characterColor);
        panelOutline.effectColor = hoverPanelColor;
        hoverOutline.SetActive(true);

        if (currentDisplay == display)
        {
            display.text = $"Player {UInumber}: <color=#{colorCode}>{characterName}</color>";
        }

        if (currentDisplay == otherDisplay)
        {
            otherDisplay.text = $"Player {UInumber}: <color=#{colorCode}>{characterName}</color>";
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panelOutline.effectColor = defaultPanelColor;
        hoverOutline.SetActive(false);

        if (currentDisplay == display && !confirmedP1)
        {
            display.text = $"Player {UInumber}:";
        }

        if (currentDisplay == otherDisplay && confirmedP1)
        {
            otherDisplay.text = $"Player {UInumber}:";
        }

    }

    public void OnMouseDown()
    {
        Debug.Log("OMD");
        if (currentDisplay == display)
        {
            Debug.Log("passedIF");
            confirmedP1 = true;
            currentDisplay = otherDisplay;
            UInumber = 2;
        }
    }
}
