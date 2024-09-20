using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float playerHealth; 
    public Movement movement;
    public Slider healthBar;
    public Animator animator;
    public bool hit;
    public bool hitDefended;
    public bool crouchBlockHit;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = healthBar.value;
        animator.SetBool("Hit", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hitDefended)
        {
            animator.SetBool("HitDefended", true);
            hit = false;
            animator.SetBool("Hit", false);
        }    

        if (crouchBlockHit)
        {
            EndHit();
        }
    }

    public void PlayerHurt(AttackSO attackSO)
    {

        if (movement.isBlocking && hit)
        {
            print("BLOCKED");
            animator.SetBool("Hit", false);
            hit = false;
            animator.Play("Base Layer.Block");
            hitDefended = true;
            animator.SetBool("HitDefended", true);
            Actions.OnPlayerHit.Invoke(1);
            Invoke("EndHit", 0.5f);
            return;
        }


        hitDefended = false;
        animator.SetBool("HitDefended", false);
        animator.SetFloat("HitAnimation", Random.Range(0, 4));
        animator.Play("Base Layer.Hit");
        healthBar.value -= attackSO.damage;
        Invoke("EndHit", 0.5f);
        Actions.OnPlayerHit.Invoke(1);
    }
    public void EndHit()
    {
        hit = false;
        animator.SetBool("Hit", false);
        Invoke("GuardDown", 0.5f);
    }

    public void GuardDown()
    {
        hitDefended = false;
        animator.SetBool("HitDefended", false);
    }
}
