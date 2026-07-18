using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int UInumber;

    public bool isSelected1;
    public bool isSelected2;

    public TMP_Text currentDisplay;
    public TMP_Text display;
    public TMP_Text otherDisplay;
    public string characterName;

    public Color characterColor;
    public Color hoverPanelColor;
    public Color defaultPanelColor;
    public Color selectedColor;

    public Color trueColor1;
    public Color trueColor2;

    public GameObject hoverOutline;
    public Outline panelOutline;

    public ConfirmSelect confirmSelect;

    // Start is called before the first frame update
    void Start()
    {
        UInumber = 1;
        currentDisplay = display;
        isSelected1 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (confirmSelect.confirmedP1)
        {
            UInumber = 2;
            currentDisplay = otherDisplay;
            selectedColor = trueColor2;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (confirmSelect.confirmedP1 && confirmSelect.confirmedP2)
        {
            return;
        }

        string colorCode = ColorUtility.ToHtmlStringRGB(characterColor);
        panelOutline.effectColor = hoverPanelColor;
        hoverOutline.SetActive(true);

        if (currentDisplay == display && !confirmSelect.confirmedP1 && !isSelected2)
        {
            display.text = $"Player {UInumber}: <color=#{colorCode}>{characterName}</color>";
        }

        if (currentDisplay == otherDisplay && !confirmSelect.confirmedP2 && !isSelected1)
        {
            otherDisplay.text = $"Player {UInumber}: <color=#{colorCode}>{characterName}</color>";
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

        if (confirmSelect.confirmedP1 && !isSelected1)
        {
            panelOutline.effectColor = defaultPanelColor;
            hoverOutline.SetActive(false);
        }

        if (confirmSelect.confirmedP2 && isSelected2)
        {
            panelOutline.effectColor = hoverPanelColor;
            hoverOutline.SetActive(true);
        }

        if (currentDisplay == display && !confirmSelect.confirmedP1)
        {
            Debug.Log("help");
            display.text = $"Player {UInumber}:";
        }

        if (currentDisplay == otherDisplay && !confirmSelect.confirmedP2)
        {
            Debug.Log("fuh");
            otherDisplay.text = $"Player {UInumber}:";
        }

    }

    public void OnMouseDown(HoverPanel hoverPanel)
    {
        if (confirmSelect.confirmedP1 && confirmSelect.confirmedP2)
        {
            return;
        }

        if (!confirmSelect.confirmedP1)
        {
            isSelected1 = true;
            hoverOutline.GetComponent<Image>().color = trueColor1;
            confirmSelect.selectedCharacter = hoverPanel.gameObject;
            ConfirmSelect.Instance.ChangeCurrentPlayerSelecting(this);
            currentDisplay = otherDisplay;
            UInumber = 2;
            selectedColor = trueColor2;
        }

        if (confirmSelect.confirmedP1 && !isSelected1)
        {
            isSelected2 = true;
            hoverOutline.GetComponent<Image>().color = trueColor2;
            confirmSelect.selectedCharacter2 = hoverPanel.gameObject;
            ConfirmSelect.Instance.ChangeCurrentPlayerSelecting(this);
        }
    }
}
