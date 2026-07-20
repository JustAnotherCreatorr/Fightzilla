using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeFight : MonoBehaviour
{
    public static InitializeFight Instance { get; private set; }

    public Material player1Material;
    public Material player2Material;

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
}