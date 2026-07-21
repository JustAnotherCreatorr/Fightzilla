using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeFight : MonoBehaviour
{
    public static InitializeFight Instance { get; private set; }

    public Material player1Material;
    public Material player2Material;

    public Color player1Color;
    public Color player2Color;

    public string player1Name;
    public string player2Name;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayer1Material(Material mat)
    {
        player1Material = mat;
    }

    public void SetPlayer2Material(Material mat)
    {
        player2Material = mat;
    }

    public void SetPlayer1Color(Color color)
    {
        player1Color = color;
    }

    public void SetPlayer2Color(Color color)
    {
        player2Color = color;
    }

    public void SetPlayer1Name(string name)
    {
        player1Name = name;
    }

    public void SetPlayer2Name(string name)
    {
        player2Name = name;
    }
}