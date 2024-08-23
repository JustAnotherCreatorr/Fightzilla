using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float playerHealth;
    public Slider healthBar;
    public Animator animator;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = healthBar.value;
        animator.SetBool("Hit", false);
    }

    // Update is called once per frame
    void Update()
    {
        bool hPressed = Input.GetKeyDown(KeyCode.H);

        TestHealth(hPressed);
    }

    public void PlayerHurt()
    {
        animator.SetBool("Hit", true);
        animator.SetFloat("HitAnimation", Random.Range(0, 4));
        animator.Play("Base Layer.Hit");
        healthBar.value -= 0.05f;
        Invoke("EndHit", 0.5f);
        Actions.OnPlayerHit.Invoke(1);
    }

    public void TestHealth(bool hPressed)
    {
        if (hPressed)
        {
            PlayerHurt();
        }
    }

    public void EndHit()
    {
        animator.SetBool("Hit", false);
    }
}
