using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prototypingmovement : MonoBehaviour
{

    /*
     Link to tutorial: https://youtu.be/KNoZeN3rjc4

    Resources Used so far:
    https://forum.unity.com/threads/how-to-make-coroutine-slow-down-based-on-timescale.889279/
    https://docs.unity3d.com/Manual/Coroutines.html
    https://stackoverflow.com/questions/38518903/unity-5-how-to-pass-multiple-parameters-on-button-click-function-from-inspector#:~:text=It%20can%20only%20take%20one%20parameter%20and,the%20function%20must%20be%20a%20non%20static%20function.
    https://forum.unity.com/threads/how-to-make-bool-trigger-an-animation-in-blend-tree.893983/
    https://forum.unity.com/threads/problems-with-backward-waliking-animation-in-blend-tree.425126/ 
     
     */

    public GameObject player;
    public float speed;
    public Animator animator;
    public int gradualIncrease = 5;
    private float animParaSpeed;
    private float acceleration;
    private bool holdingDown;
    private float maxSpeed = 1;
    private float maxNegativeSpeed = -1;
    private float runAccel = 1;
    public Rigidbody rigidbody;

    public Transform orientation;
    public float dashForce;

    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool shiftPressed = Input.GetKey(KeyCode.RightShift);
        bool dashPressed = Input.GetKeyDown(KeyCode.Comma);
        bool upArrowPressed = Input.GetKeyDown(KeyCode.UpArrow);

        Sprint(shiftPressed);

        Dash(dashPressed);

        Jump(upArrowPressed);

        // if right is being pressed

        if (Input.anyKey && horizontal != 0f)
        {
            float posSpeed = Mathf.Abs(animParaSpeed); 
            acceleration = posSpeed + Time.deltaTime + 0.1f;
            float PosHorizontal = Mathf.Abs(horizontal);
            posSpeed = PosHorizontal * acceleration * runAccel;
            holdingDown = true;
            animParaSpeed = posSpeed;
        }

        // no keys are being pressed

        if (!Input.anyKey && holdingDown)
        {
            if (horizontal != 0f)
            {
               // Debug.Log("A key was released");
                holdingDown = false;
                acceleration = 0f;
                StartCoroutine(GradualDecrease());
            }
            
        }

        if (horizontal > 0)
        {
            animParaSpeed = Mathf.Abs(animParaSpeed);
        } 
        else if (horizontal < 0)
        {
            animParaSpeed = -Mathf.Abs(animParaSpeed);
        }

        animParaSpeed = Mathf.Clamp(animParaSpeed, maxNegativeSpeed, maxSpeed);

        Vector3 movement = new Vector3(0, vertical, horizontal);

        transform.position += movement * Time.deltaTime * speed;

        animator.SetFloat("MoveSpeed", animParaSpeed);
    
    }

    private void Sprint(bool shiftPressed)
    {
        if (shiftPressed)
        {
            maxSpeed = 2;
            maxNegativeSpeed = -2;
            speed = 10;
            runAccel = 1.07f;
        }
        else
        {
            maxSpeed = 1;
            maxNegativeSpeed = -1;
            speed = 5;
            runAccel = 1;
        }
    }

    private void Dash(bool dashPressed)
    {
        if (dashPressed && animParaSpeed > 0f)
        {

            Vector3 forceToApply = orientation.forward * dashForce;
            rigidbody.AddForce(forceToApply, ForceMode.Impulse);
            animator.SetBool("dashPressed", true);
            animator.Play("Base Layer.DashForward");
            animator.SetBool("dashPressed", false);
        }

        if (dashPressed && animParaSpeed < 0f)
        {
            Vector3 forceToApply = orientation.forward * dashForce * -1f;
            rigidbody.AddForce(forceToApply, ForceMode.Impulse);
            animator.SetBool("dashPressed", true);
            animator.Play("Base Layer.DashBackward");
            animator.SetBool("dashPressed", false);
        }
    }

    private void Jump(bool spacePressed)
    {
        if (spacePressed)
        {
            animator.SetBool("spacePressed", true);
            animator.Play("Base Layer.Jump");
            animator.SetBool("spacePressed", false);
        }
    }

    IEnumerator GradualDecrease()
     {
        float posSpeed = Mathf.Abs(animParaSpeed);

        while (posSpeed > 0f)
        {
            posSpeed -= 0.2f * Time.deltaTime * gradualIncrease;
            animParaSpeed = posSpeed;
            yield return new WaitForEndOfFrame();
        }

        if (posSpeed < 0f && !Input.GetKey(KeyCode.LeftArrow))
        {
            animParaSpeed = 0f;
        }
     } 
}
