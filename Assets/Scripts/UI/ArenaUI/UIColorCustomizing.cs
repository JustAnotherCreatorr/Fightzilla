using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorCustomizing : MonoBehaviour
{
    public GameObject backgroundImage;
    public GameObject colorImage;
    public GameObject O1;
    public GameObject O2;
    public GameObject F1;
    public GameObject F2;
    public Text characterName;

    public GameObject backgroundImage2;
    public GameObject colorImage2;
    public GameObject O3;
    public GameObject O4;
    public GameObject F3;
    public GameObject F4;
    public Text characterName2;

    private const string VoidCharacterName = "Void";

    // Start is called before the first frame update
    void Start()
    {
        ApplyPlayerHud(InitializeFight.Instance.player1Color, InitializeFight.Instance.player1Name, colorImage, O1, O2, F1, F2, backgroundImage, characterName);

        ApplyPlayerHud(InitializeFight.Instance.player2Color, InitializeFight.Instance.player2Name, colorImage2, O3, O4, F3, F4, backgroundImage2, characterName2);
    }

    private void ApplyPlayerHud(Color characterColor, string charName, GameObject portrait, GameObject outlineA, GameObject outlineB, GameObject fillA, GameObject fillB, GameObject background, Text nameText)
    {
        portrait.GetComponent<Image>().color = characterColor;

        bool isVoid = charName == VoidCharacterName;

        Color fillColor = isVoid ? Color.black : characterColor;
        fillA.GetComponent<Image>().color = fillColor;
        fillB.GetComponent<Image>().color = fillColor;

        if (isVoid)
        {
            outlineA.GetComponent<Image>().color = Color.white;
            outlineB.GetComponent<Image>().color = Color.white;
        }

        nameText.text = charName;

        if (isVoid)
        {
            background.GetComponent<Image>().color = Color.white;
        }
    }
}
