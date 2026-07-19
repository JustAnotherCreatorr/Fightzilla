using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    public int playerNumber;

    public GameObject startButton;

    public void OnMouseDown()
    {
        if (playerNumber == 1 && ConfirmSelect.Instance.confirmedP1)
        {
            HoverPanel hoverPanel = ConfirmSelect.Instance.selectedCharacter.GetComponent<HoverPanel>();
            hoverPanel.isSelected1 = false;
            hoverPanel.hoverOutline.SetActive(false);
            hoverPanel.panelOutline.effectColor = hoverPanel.defaultPanelColor;
            hoverPanel.display.text = "Player 1:";

            ConfirmSelect.Instance.confirmedP1 = false;
            ConfirmSelect.Instance.selectedCharacter = null;

            if (ConfirmSelect.Instance.confirmedP2)
            {
                HoverPanel hoverPanel2 = ConfirmSelect.Instance.selectedCharacter2.GetComponent<HoverPanel>();
                hoverPanel2.isSelected2 = false;
                hoverPanel2.hoverOutline.SetActive(false);
                hoverPanel2.panelOutline.effectColor = hoverPanel2.defaultPanelColor;
                hoverPanel2.otherDisplay.text = "Player 2:";

                ConfirmSelect.Instance.selectedCharacter2 = null;
                ConfirmSelect.Instance.confirmedP2 = false;

                if (ConfirmSelect.Instance.button2 != null)
                {
                    ConfirmSelect.Instance.button2.SetActive(false);
                }
            }

            gameObject.SetActive(false);
        }

        if (playerNumber == 2 && ConfirmSelect.Instance.confirmedP2)
        {
            HoverPanel hoverPanel2 = ConfirmSelect.Instance.selectedCharacter2.GetComponent<HoverPanel>();
            hoverPanel2.isSelected2 = false;
            hoverPanel2.hoverOutline.SetActive(false);
            hoverPanel2.panelOutline.effectColor = hoverPanel2.defaultPanelColor;
            hoverPanel2.otherDisplay.text = "Player 2:";

            ConfirmSelect.Instance.selectedCharacter2 = null;
            ConfirmSelect.Instance.confirmedP2 = false;
            gameObject.SetActive(false);
        }

        startButton.SetActive(false);

    }
}

