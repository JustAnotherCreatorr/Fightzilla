using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text display;
    private int UInumber;
    public string characterName;
    public Color characterColor;

    // Start is called before the first frame update
    void Start()
    {
        UInumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string colorCode = ColorUtility.ToHtmlStringRGB(characterColor);

        display.text = $"Player {UInumber}: <color=#{colorCode}>{characterName}</color>";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        display.text = $"Player {UInumber}:";
    }
}
