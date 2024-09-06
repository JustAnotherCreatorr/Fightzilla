using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{

    public GameObject player;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool lightPunch = Input.GetKeyDown(KeyCode.H);
        bool medPunch = Input.GetKeyDown(KeyCode.J);
        bool heavyPunch = Input.GetKeyDown(KeyCode.K);
        bool lightKick = Input.GetKeyDown(KeyCode.B);
        bool medKick = Input.GetKeyDown(KeyCode.N);
        bool heavyKick = Input.GetKeyDown(KeyCode.M);

        LPunch(lightPunch); 
        MPunch(medPunch); 
        HPunch(heavyPunch); 
        LKick(lightKick); 
        MKick(medKick); 
        HKick(heavyKick);
    }

    void LPunch(bool lightPunch)
    {
        if (!Input.GetKeyDown(KeyCode.H))
        {
            return; 
        }
            animator.SetBool("LPunch", true);
            animator.Play("Base Layer.LPunch");
            animator.SetBool("LPunch", false);
    }

    void MPunch(bool medPunch)
    {
        if (!Input.GetKeyDown(KeyCode.J))
        {
            return;
        }
        animator.SetBool("MPunch", true);
        animator.Play("Base Layer.MPunch");
        animator.SetBool("MPunch", false);
    }

    void HPunch(bool heavyPunch)
    {
        if (!Input.GetKeyDown(KeyCode.K))
        {
            return;
        }
        animator.SetBool("HPunch", true);
        animator.Play("Base Layer.HPunch");
        animator.SetBool("HPunch", false);
    }

    void LKick(bool lightKick)
    {
        if (!Input.GetKeyDown(KeyCode.B))
        {
            return;
        }
        animator.SetBool("LKick", true);
        animator.Play("Base Layer.LKick");
        animator.SetBool("LKick", false);
    }

    void MKick(bool medKick)
    {
        if (!Input.GetKeyDown(KeyCode.N))
        {
            return;
        }
        animator.SetBool("MKick", true);
        animator.Play("Base Layer.MKick");
        animator.SetBool("MKick", false);
    }

    void HKick(bool heavyKick)
    {
        if (!Input.GetKeyDown(KeyCode.M))
        {
            return;
        }
        animator.SetBool("HKick", true);
        animator.Play("Base Layer.HKick");
        animator.SetBool("HKick", false);
    }
 
}
