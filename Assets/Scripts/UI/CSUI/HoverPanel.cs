using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // isSelectedX means "this specific panel is the one player X picked".
    // This is fine to keep per-panel since it's really about THIS panel.
    public bool isSelected1;
    public bool isSelected2;

    public TMP_Text display;      // Player 1's display text
    public TMP_Text otherDisplay; // Player 2's display text

    public string characterName;
    public Color characterColor;
    public Material characterMaterial;
    public Color hoverPanelColor;
    public Color defaultPanelColor;
    public Color trueColor1;
    public Color trueColor2;
    public Color defaultHoverOutlineColor;

    public GameObject hoverOutline;
    public Outline panelOutline;
    public ConfirmSelect confirmSelect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (confirmSelect.confirmedP1 && confirmSelect.confirmedP2)
        {
            return;
        }

        // Don't show "available to pick" feedback on a panel that's already taken.
        if (isSelected1 || isSelected2)
        {
            return;
        }

        string colorCode = ColorUtility.ToHtmlStringRGB(characterColor);
        panelOutline.effectColor = hoverPanelColor;
        hoverOutline.SetActive(true);

        // Whose turn is it, globally? Every panel checks the same source of truth.
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

        // If this panel is the locked-in pick for a confirmed player, keep it highlighted.
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

    // Keeping the original signature/parameter — this is almost certainly wired
    // in the Inspector via an EventTrigger's PointerClick with a static parameter
    // pointing at this exact method, so changing the signature breaks that binding.
    public void OnMouseDown(HoverPanel hoverPanel)
    {
        if (confirmSelect.confirmedP1 && confirmSelect.confirmedP2)
        {
            return;
        }

        // A panel already claimed by one player can't be picked by the other.
        if (isSelected1 || isSelected2)
        {
            return;
        }

        if (!confirmSelect.confirmedP1)
        {
            isSelected1 = true;
            hoverOutline.GetComponent<Image>().color = trueColor1;
            confirmSelect.ConfirmPlayer1(hoverPanel.gameObject);
            InitializeFight.Instance.SetPlayer1Material(characterMaterial);
            InitializeFight.Instance.SetPlayer1Color(characterColor);
            InitializeFight.Instance.SetPlayer1Name(characterName);
        }
        else if (!confirmSelect.confirmedP2)
        {
            isSelected2 = true;
            hoverOutline.GetComponent<Image>().color = trueColor2;
            confirmSelect.ConfirmPlayer2(hoverPanel.gameObject);
            InitializeFight.Instance.SetPlayer2Material(characterMaterial);
            InitializeFight.Instance.SetPlayer2Color(characterColor);
            InitializeFight.Instance.SetPlayer2Name(characterName);
        }
    }
}
