using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{

    public int playerNumber;
    public Animator animator;

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

        if (playerNumber == 1)
        {
            lightPunch = Input.GetKeyDown(KeyCode.H);
            medPunch = Input.GetKeyDown(KeyCode.J);
            heavyPunch = Input.GetKeyDown(KeyCode.K);
            lightKick = Input.GetKeyDown(KeyCode.B);
            medKick = Input.GetKeyDown(KeyCode.N);
            heavyKick = Input.GetKeyDown(KeyCode.M);
        }

        if (playerNumber == 2)
        {
            lightPunch = Input.GetKeyDown(KeyCode.Alpha1);
            medPunch = Input.GetKeyDown(KeyCode.Alpha2);
            heavyPunch = Input.GetKeyDown(KeyCode.Alpha3);
            lightKick = Input.GetKeyDown(KeyCode.E);
            medKick = Input.GetKeyDown(KeyCode.R);
            heavyKick = Input.GetKeyDown(KeyCode.T);
        }

        if (lightPunch) LPunch(lightPunch);
        if (medPunch) MPunch(medPunch);
        if (heavyPunch) HPunch(heavyPunch);
        if (lightKick) LKick(lightKick);
        if (medKick) MKick(medKick);
        if (heavyKick) HKick(heavyKick);
    }

    void LPunch(bool lightPunch)
    {
        animator.SetBool("LPunch", true);
        animator.Play("Base Layer.LPunch");
        animator.SetBool("LPunch", false);
    }

    void MPunch(bool medPunch)
    {
        animator.SetBool("MPunch", true);
        animator.Play("Base Layer.MPunch");
        animator.SetBool("MPunch", false);
    }

    void HPunch(bool heavyPunch)
    {
        animator.SetBool("HPunch", true);
        animator.Play("Base Layer.HPunch");
        animator.SetBool("HPunch", false);
    }

    void LKick(bool lightKick)
    {
        animator.SetBool("LKick", true);
        animator.Play("Base Layer.LKick");
        animator.SetBool("LKick", false);
    }

    void MKick(bool medKick)
    {
        animator.SetBool("MKick", true);
        animator.Play("Base Layer.MKick");
        animator.SetBool("MKick", false);
    }

    void HKick(bool heavyKick)
    {
        animator.SetBool("HKick", true);
        animator.Play("Base Layer.HKick");
        animator.SetBool("HKick", false);
    }

}
