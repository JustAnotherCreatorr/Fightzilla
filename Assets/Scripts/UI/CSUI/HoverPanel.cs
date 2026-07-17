using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int UInumber;

    public bool confirmedP1;

    public TMP_Text currentDisplay;
    public TMP_Text display;
    public TMP_Text otherDisplay;
    public string characterName;

    public Color characterColor;
    public Color hoverPanelColor;
    public Color defaultPanelColor;

    public GameObject selectedCharacter;

    public GameObject hoverOutline;
    public Outline panelOutline;

    public P1ConfirmSelect p1Confirm;

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
        if (p1Confirm.confirmedP1)
        {
            currentDisplay = otherDisplay;
            UInumber = 2;
        }
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

    public void OnMouseDown(HoverPanel hoverPanel)
    { 
        selectedCharacter = hoverPanel.gameObject;
        P1ConfirmSelect.Instance.ChangeCurrentPlayerSelecting(this);
        currentDisplay = otherDisplay;
        UInumber = 2;
    }
}
