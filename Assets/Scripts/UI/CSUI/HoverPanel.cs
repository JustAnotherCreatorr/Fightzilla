using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isSelected1;
    public bool isSelected2;

    public TMP_Text display;     
    public TMP_Text otherDisplay; 

    public string characterName;
    public Color characterColor;
    public Color hoverPanelColor;
    public Color defaultPanelColor;
    public Color trueColor1;
    public Color trueColor2;

    public GameObject hoverOutline;
    public Outline panelOutline;
    public ConfirmSelect confirmSelect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (confirmSelect.confirmedP1 && confirmSelect.confirmedP2)
        {
            return;
        }

        string colorCode = ColorUtility.ToHtmlStringRGB(characterColor);
        panelOutline.effectColor = hoverPanelColor;
        hoverOutline.SetActive(true);

        if (isSelected1 || isSelected2)
        {
            return;
        }
      
        if (!confirmSelect.confirmedP1)
        {
            display.text = $"Player 1: <color=#{colorCode}>{characterName}</color>";
        }
        else if (!confirmSelect.confirmedP2)
        {
            otherDisplay.text = $"Player 2: <color=#{colorCode}>{characterName}</color>";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panelOutline.effectColor = defaultPanelColor;
        hoverOutline.SetActive(false);

        if (confirmSelect.confirmedP1 && isSelected1)
        {
            panelOutline.effectColor = hoverPanelColor;
            hoverOutline.SetActive(true);
        }
        if (confirmSelect.confirmedP2 && isSelected2)
        {
            panelOutline.effectColor = hoverPanelColor;
            hoverOutline.SetActive(true);
        }

        if (!confirmSelect.confirmedP1)
        {
            display.text = "Player 1:";
        }
        else if (!confirmSelect.confirmedP2)
        {
            otherDisplay.text = "Player 2:";
        }
    }

    public void OnMouseDown(HoverPanel hoverPanel)
    {
        if (confirmSelect.confirmedP1 && confirmSelect.confirmedP2)
        {
            return;
        }

        if (isSelected1 || isSelected2)
        {
            return;
        }

        if (!confirmSelect.confirmedP1)
        {
            isSelected1 = true;
            hoverOutline.GetComponent<Image>().color = trueColor1;
            confirmSelect.ConfirmPlayer1(hoverPanel.gameObject);
        }
        else if (!confirmSelect.confirmedP2)
        {
            isSelected2 = true;
            hoverOutline.GetComponent<Image>().color = trueColor2;
            confirmSelect.ConfirmPlayer2(hoverPanel.gameObject);
        }
    }
}
