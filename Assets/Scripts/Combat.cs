using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Combat : MonoBehaviour
{
    public int playerNumber;
    public Animator animator;
    public string chosenAttack;
    public bool animInPlay;
    private float delayLength;
    public bool allowCombat;
    public PauseMenu pm;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool lightPunch = false;
        bool medPunch = false;
        bool heavyPunch = false;
        bool lightKick = false;
        bool medKick = false;
        bool heavyKick = false;

        if (playerNumber == 2)
        {
            lightPunch = Input.GetKeyDown(KeyCode.Keypad4);
            medPunch = Input.GetKeyDown(KeyCode.Keypad5);
            heavyPunch = Input.GetKeyDown(KeyCode.Keypad6);
            lightKick = Input.GetKeyDown(KeyCode.Keypad7);
            medKick = Input.GetKeyDown(KeyCode.Keypad8);
            heavyKick = Input.GetKeyDown(KeyCode.Keypad9);
        }
        if (playerNumber == 1)
        {
            lightPunch = Input.GetKeyDown(KeyCode.Alpha4);
            medPunch = Input.GetKeyDown(KeyCode.Alpha5);
            heavyPunch = Input.GetKeyDown(KeyCode.Alpha6);
            lightKick = Input.GetKeyDown(KeyCode.R);
            medKick = Input.GetKeyDown(KeyCode.T);
            heavyKick = Input.GetKeyDown(KeyCode.Y);
        }
        if (delayLength > 0)
        {
            delayLength -= Time.deltaTime;
            return;
        }
        if (allowCombat == false)
        {
            return;
        }
        if (pm.paused)
        {
            return;
        }
        if (lightPunch) LPunch(lightPunch);
        if (medPunch) MPunch(medPunch);
        if (heavyPunch) HPunch(heavyPunch);
        if (lightKick) LKick(lightKick);
        if (medKick) MKick(medKick);
        if (heavyKick) HKick(heavyKick);
    }

    private void LPunch(bool lightPunch)
    {
        animator.SetBool("LPunch", true);
        animator.Play("Base Layer.LPunch");
        animator.SetBool("LPunch", false);
        delayLength = 0.37f;

    }

    private void MPunch(bool medPunch)
    {
        animator.SetBool("MPunch", true);
        animator.Play("Base Layer.MPunch");
        animator.SetBool("MPunch", false);
        delayLength = 0.45f;
    }

    private void HPunch(bool heavyPunch)
    {
        animator.SetBool("HPunch", true);
        animator.Play("Base Layer.HPunch");
        animator.SetBool("HPunch", false);
        delayLength = 0.5f; 

    }
    private void LKick(bool lightKick)
    {
        animator.SetBool("LKick", true);
        animator.Play("Base Layer.LKick");
        animator.SetBool("LKick", false);
        delayLength = 0.37f; 
    }
    private void MKick(bool medKick)
    {
        animator.SetBool("MKick", true);
        animator.Play("Base Layer.MKick");
        animator.SetBool("MKick", false);
        delayLength = 0.45f;

    }
    private void HKick(bool heavyKick)
    {
        animator.SetBool("HKick", true);
        animator.Play("Base Layer.HKick");
        animator.SetBool("HKick", false);
        delayLength = 0.5f; 

    }
}
