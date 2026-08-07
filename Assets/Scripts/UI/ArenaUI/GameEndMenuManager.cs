using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndMenuManager : MonoBehaviour
{
    public GameObject GEM;
    public GameObject GEMbox;
    public Outline GEMplayerWon;
    public Text whichPlayerWin;
    public Text extraMessage;
    public GameObject resume;
    public GameObject PA;
    public GameObject PAtext;
    public GameObject mainMenuButton;
    public GameObject MMtext;
    public Color draw;
    public Color normalButtonColor;
    public bool GEMActive;
    public bool settingUpNR;

    public void ShowGameEndMenu(bool isDraw, bool otherPlayerIsWinner, int playerNumber)
    {
        GEMActive = true;

        if (isDraw)
        {
            GEM.SetActive(true);
            PA.SetActive(true);
            resume.SetActive(false);
            whichPlayerWin.color = draw;
            whichPlayerWin.text = "DRAW!";
            GEMplayerWon.effectColor = draw;
            extraMessage.text = "The fight never ends!";
        }

        if (otherPlayerIsWinner && playerNumber == 1)
        {
            GEM.SetActive(true);
            PA.SetActive(true);
            resume.SetActive(false);
            whichPlayerWin.color = InitializeFight.Instance.player2Color;
            whichPlayerWin.text = "Player 2 wins!";
            GEMplayerWon.effectColor = InitializeFight.Instance.player2Color;
            bool p2IsVoid = InitializeFight.Instance.player2Name == "Void";
            GEMbox.GetComponent<Image>().color = p2IsVoid ? Color.white : Color.black;
            PAtext.GetComponent<Text>().color = p2IsVoid ? Color.black : Color.white;
            PA.GetComponent<Outline>().effectColor = p2IsVoid ? Color.black : Color.white;
            MMtext.GetComponent<Text>().color = p2IsVoid ? Color.black : Color.white;
            mainMenuButton.GetComponent<Outline>().effectColor = p2IsVoid ? Color.black : Color.white;
            PA.GetComponent<Image>().color = p2IsVoid ? Color.white : normalButtonColor;
            mainMenuButton.GetComponent<Image>().color = p2IsVoid ? Color.black : normalButtonColor;
            extraMessage.text = "Dare to fight again?";
        }

        if (otherPlayerIsWinner && playerNumber == 2)
        {
            GEM.SetActive(true);
            PA.SetActive(true);
            resume.SetActive(false);
            whichPlayerWin.color = InitializeFight.Instance.player1Color;
            whichPlayerWin.text = "Player 1 wins!";
            GEMplayerWon.effectColor = InitializeFight.Instance.player1Color;
            bool p1IsVoid = InitializeFight.Instance.player1Name == "Void";
            GEMbox.GetComponent<Image>().color = p1IsVoid ? Color.white : Color.black;
            PAtext.GetComponent<Text>().color = p1IsVoid ? Color.black : Color.white;
            PA.GetComponent<Outline>().effectColor = p1IsVoid ? Color.black : Color.white;
            MMtext.GetComponent<Text>().color = p1IsVoid ? Color.black : Color.white;
            mainMenuButton.GetComponent<Outline>().effectColor = p1IsVoid ? Color.black : Color.white;
            PA.GetComponent<Image>().color = p1IsVoid ? Color.white : normalButtonColor;
            mainMenuButton.GetComponent<Image>().color = p1IsVoid ? Color.white : normalButtonColor;
            extraMessage.text = "Dare to fight again?";
        }
    }
}