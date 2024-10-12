using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{

    public GameObject player;
    public Animator animator;
    public Rigidbody rigidbody;
    private float animParaSpeed = 1;


    bool dashPressed = false;

    public float dashCd;
    private float dashCdTimer;

    public Transform orientation;
    public float dashForce;
    public float dashDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dashPressed = Input.GetKeyDown(KeyCode.Comma);
        Dashs(dashPressed);

        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    public void Dashs(bool dashPressed)
    {

        animator.SetFloat("MoveSpeed", animParaSpeed);

        if (dashPressed && animParaSpeed > 0f)
        {

            if (dashCdTimer > 0)
            {
                print("timer is above 0");
                return;
            }
            else dashCdTimer = dashCd;

            Vector3 forceToApply = orientation.forward * dashForce;
        
            rigidbody.AddForce(forceToApply, ForceMode.Impulse);
        
            animator.SetBool("dashPressed", true);
            animator.Play("Base Layer.DashForward");
            animator.SetBool("dashPressed", false);
         

        }

        if (dashPressed && animParaSpeed < 0f)
        {
            if (dashCdTimer > 0)
            {
                print("timer is above 0");
                return;
            }
            else
            {
                dashCdTimer = dashCd;
            }

            Vector3 forceToApply = orientation.forward * dashForce * -1f;
           
            rigidbody.AddForce(forceToApply, ForceMode.Impulse);
        
            animator.SetBool("dashPressed", true);
            animator.Play("Base Layer.DashBackward");
            animator.SetBool("dashPressed", false);
        
        }
    }
}
