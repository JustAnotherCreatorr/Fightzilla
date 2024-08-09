using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prototypingmovement : MonoBehaviour
{

    #region variables

    public GameObject player;
    public Animator animator;
    public Rigidbody rigidbody;

    public float speed;
    public int gradualIncrease = 5;
    private float animParaSpeed;
    private float acceleration;
    private bool holdingDown;
    private float maxParaSpeed = 1;
    private float maxParaNegativeSpeed = -1;
    private float runAccel = 1;

    public float dashCd;
    private float dashCdTimer;

    public Transform orientation;
    public float dashForce;
    public float dashDuration;

    public float jumpStrength;
    public bool isGrounded = true;

    public bool isSprinting;
    public bool isCrouching;

    private float backwardSpeedMod;
    private float crouchSpeedMod;
    private float slightBoostMod;
    private float pullback;

    #endregion variables

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator.SetFloat("YVelocity", rigidbody.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        #region movement

        float horizontal = Input.GetAxis("Horizontal");
        bool shiftPressed = Input.GetKey(KeyCode.RightShift);
        bool downPressed = Input.GetKey(KeyCode.DownArrow);
        bool dashPressed = Input.GetKeyDown(KeyCode.Comma);
        bool upArrowPressed = Input.GetKeyDown(KeyCode.UpArrow);
        GetIsGrounded();

        Sprint(shiftPressed, horizontal);

        Dash(dashPressed);

        Jump(upArrowPressed);

        Crouch(downPressed);

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

        animParaSpeed = Mathf.Clamp(animParaSpeed, maxParaNegativeSpeed, maxParaSpeed);

        Vector3 movement = new Vector3(0, rigidbody.velocity.x, horizontal);

        transform.position += movement * Time.deltaTime * speed * crouchSpeedMod * backwardSpeedMod * slightBoostMod * pullback;

        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }

        animator.SetFloat("MoveSpeed", animParaSpeed);

        animator.SetFloat("YVelocity", rigidbody.velocity.y);


        #endregion movement

        #region SpeedMods

        if (horizontal > 0 && !isCrouching)
        {
            backwardSpeedMod = 0;
            crouchSpeedMod = 0;
        }

        if (horizontal > 0 && !isSprinting && isGrounded)
        {
            slightBoostMod = 1.4f;
        }
        else
        {
            slightBoostMod = 0.9f;
        }

        if (horizontal < 0)
        {
            if (!isGrounded)
            {
                backwardSpeedMod = 1;
                return;
            }
            backwardSpeedMod = 0.6f;
        }
        else
        {
            backwardSpeedMod = 1;
        }

        if (!isGrounded && horizontal > 0)
        {
            pullback = 0.7f;
        } else
        {
            pullback = 1;
        }

        #endregion SpeedMods

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "groundingFloor")
        {
            animator.SetBool("isGrounded", true);
            animator.Play("Base Layer.Blend Tree");
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    private bool GetIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.black);
        
        if (isGrounded)
        {
            print("if is grounded");
            animator.SetBool("isGrounded", true);
            backwardSpeedMod = 0.6f;
        } 
        else
        {
            if (rigidbody.velocity.y < 0)
            {
                animator.SetTrigger("isFalling");
                animator.Play("Base Layer.Falling");
            }
        }

        return isGrounded;
    }

    private void Sprint(bool shiftPressed, float horizontal)
    {

        if (isCrouching)
        {
            isSprinting = false;
            return;
        }

        if (!isGrounded)
        {
            isSprinting = false;
            return;
        }

        if (shiftPressed)
        {
            isSprinting = true;
            maxParaSpeed = 2;
            maxParaNegativeSpeed = -2;

            if (horizontal > 0)
            {
                speed = 15;

            } else
            {
                speed = 10;
            }

            runAccel = 1.07f;
        }
        else
        {
            isSprinting = false;
            maxParaSpeed = 1;
            maxParaNegativeSpeed = -1;
            speed = 5;
            runAccel = 1;
        }
    }

    private void Crouch(bool downpressed)
    {

        if (!isGrounded)
        {
            crouchSpeedMod = 1;
            return;
        }

        if (!downpressed)
        {
            isCrouching = false;
            crouchSpeedMod = 1f;
            animator.SetBool("isCrouching", false);
            return;
        }

        if (downpressed)
        {
            isCrouching = true;
            crouchSpeedMod = 0.8f;
            animator.SetBool("isCrouching", true);
            animator.Play("Base Layer.Crouch");
        }
    }

    public void Dash(bool dashPressed)
    {

        if (!isGrounded)
        {
            return;
        }

        if (isCrouching)
        {
            return;
        }

        if (dashPressed && animParaSpeed > 0f)
        {
            if (dashCdTimer > 0)
            {
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
                return;
            }
            else dashCdTimer = dashCd;
            Vector3 forceToApply = orientation.forward * dashForce * -1f;
            rigidbody.AddForce(forceToApply, ForceMode.Impulse);
            animator.SetBool("dashPressed", true);
            animator.Play("Base Layer.DashBackward");
            animator.SetBool("dashPressed", false);
        }
    }

    private void Jump(bool upArrowPressed)
    {


        if (!isGrounded)
        {
            return;
        }

        if (!upArrowPressed)
        {
            return;
        }

            print("jump");
            animator.Play("Base Layer.Jumping");
            backwardSpeedMod = 1f;
            rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            animator.SetBool("isGrounded", false);
    }

    IEnumerator GradualDecrease()
     {
        float posSpeed = Mathf.Abs(animParaSpeed);

        while (posSpeed > 0f)
        {
            posSpeed -= 0.4f * Time.deltaTime * gradualIncrease;
            animParaSpeed = posSpeed;
            yield return new WaitForEndOfFrame();
        }

        if (posSpeed < 0f && !Input.GetKey(KeyCode.LeftArrow))
        {
            animParaSpeed = 0f;
        }
    } 

}
