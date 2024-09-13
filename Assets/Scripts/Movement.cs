using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{



    #region variables

    public bool debug;

    public GameObject player1;
    public GameObject player2;
    public Animator animator;
    public Rigidbody rigidbody;
    public PlayerHealth playerHealth;
    public int playerNumber;

    public float speed;
    public int gradualIncrease = 5;
    private float animParaSpeed;
    private float acceleration;
    private bool holdingDown;
    private float maxParaSpeed = 1f;
    private float maxParaNegativeSpeed = -1f;
    private float runAccel = 1f;

    public float dashCd;
    private float dashCdTimer;

    public Transform orientation;
    public float dashForce;
    public float dashDuration;

    public float jumpStrength;
    public bool isGrounded = true;

    public bool isSprinting;
    public bool isCrouching;
    public bool isBlocking;

    private float backwardSpeedMod;
    private float crouchSpeedMod = 1f;
    private float slightBoostMod = 1f;
    private float pullback = 1f;
    private float hitStop = 1f;

    private float mirrorPlayerFix = 0f;

    private float initialLocalScaleZ;

    public AnimationClip[] randomHitAnimations;

    private bool allowMovement = false;

    private float prevPos;
    private float currentPos;
    private int direction;

    #endregion variables

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator.SetFloat("YVelocity", rigidbody.velocity.y);
        prevPos = transform.position.z;
        currentPos = transform.position.z;
        initialLocalScaleZ = transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {

        if (allowMovement == false)
        {
            return;
        }

        #region movement


        float horizontal = 0;
        bool shiftPressed = false;
        bool downPressed = false;
        bool dashPressed = false;
        bool upArrowPressed = false;

        bool cancelSprint = false;
        bool cancelCrouch = false;

        if (playerNumber == 1)
        {
             horizontal = Input.GetAxis("Horizontal");
             shiftPressed = Input.GetKey(KeyCode.RightShift);
             cancelSprint = Input.GetKeyUp(KeyCode.RightShift);
             downPressed = Input.GetKey(KeyCode.DownArrow);
            cancelCrouch = Input.GetKeyUp(KeyCode.DownArrow);
            dashPressed = Input.GetKeyDown(KeyCode.Comma);
             upArrowPressed = Input.GetKeyDown(KeyCode.UpArrow);
        }
        
        if (playerNumber == 2)
        {
            horizontal = Input.GetAxis("Horizontal2");
            shiftPressed = Input.GetKey(KeyCode.LeftShift);
            cancelSprint = Input.GetKeyUp(KeyCode.LeftShift);
            downPressed = Input.GetKey(KeyCode.S);
            cancelCrouch = Input.GetKeyUp(KeyCode.S);
            dashPressed = Input.GetKeyDown(KeyCode.Q);
            upArrowPressed = Input.GetKeyDown(KeyCode.W);
        }



        GetIsGrounded();
        Reverse();

        prevPos = currentPos;
        currentPos = transform.position.z;

        if (currentPos > prevPos)
        {
            direction = 1;
        }

        if (currentPos < prevPos)
        {
            direction = -1;
        }


        if (shiftPressed)
        {
            Sprint(shiftPressed, horizontal);
        } else if (cancelSprint)
        {
            CancelSprint();
        }

        if (dashPressed)
        {
            Dash(dashPressed);
        }

        if (upArrowPressed)
        {
            Jump(upArrowPressed);
        }

        if (downPressed)
        {
            Crouch(downPressed);
        } else if (cancelCrouch)
        {
            CancelCrouch();
        }

        // if right is being pressed

        if (Input.anyKey && horizontal != 0f)
        {
            float posSpeed = Mathf.Abs(animParaSpeed);
            posSpeed *= -1;
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
            if (playerNumber == 2)
            {
                if (isSprinting)
                {
                    mirrorPlayerFix = 2f;
                } else
                {
                    mirrorPlayerFix = 1f;
                }

                float decimalValue = animParaSpeed -= mirrorPlayerFix;
                animParaSpeed = animParaSpeed += decimalValue;
            }
        }
        else if (horizontal < 0)
        {
            animParaSpeed = -Mathf.Abs(animParaSpeed);

            if (playerNumber == 2)
            {
                if (isSprinting)
                {
                    mirrorPlayerFix = -2f;
                }
                else
                {
                    mirrorPlayerFix = -1f;
                }

                float decimalValueN = animParaSpeed -= mirrorPlayerFix;
                animParaSpeed = animParaSpeed += decimalValueN;
            }

            float decimalValue = animParaSpeed -= mirrorPlayerFix;
            animParaSpeed = animParaSpeed += decimalValue;
        }

        animParaSpeed = Mathf.Clamp(animParaSpeed, maxParaNegativeSpeed, maxParaSpeed);

        Vector3 movement = new Vector3(0, rigidbody.velocity.x, horizontal);

        if (debug)
        {
            Debug.Log($"movement: {movement}, speed: {speed}, crouchMod {crouchSpeedMod}, backwardMod: {backwardSpeedMod}, slightBoost: {slightBoostMod}, pullback: {pullback}, hitstop: {hitStop}");
        }
        transform.position += movement * Time.deltaTime * speed * crouchSpeedMod * backwardSpeedMod * slightBoostMod * pullback * hitStop;

        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }

        animator.SetFloat("MoveSpeed", animParaSpeed);

        animator.SetFloat("YVelocity", rigidbody.velocity.y);

        if (animParaSpeed != 0)
        {
            if (horizontal > 0)
            {
                animParaSpeed += 1;
            }

            if (horizontal < 0)
            {
                animParaSpeed += -1;
            }
        }

        #endregion movement

        #region SpeedMods

        if (horizontal > 0 && !isCrouching)
        {
            backwardSpeedMod = 1;
            crouchSpeedMod = 1;
        }

        if (playerNumber == 1)
        {
            if (horizontal > 0 && !isSprinting && isGrounded)
            {
                slightBoostMod = 1.4f;
            }

            if (horizontal < 0 || isSprinting || !isGrounded)
            {
                slightBoostMod = 0.9f;
            }
        } else if (playerNumber == 2)
        {
            if (horizontal < 0 && !isSprinting && isGrounded)
            {
                slightBoostMod = 1.4f;
            }

            if (horizontal > 0 || isSprinting || !isGrounded)
            {
                slightBoostMod = 0.9f;
            }
        }

        if (horizontal < 0 && playerNumber == 1)
        {
            if (!isGrounded)
            {
                backwardSpeedMod = 1;
                animator.SetBool("isBlocking", false);
                isBlocking = false;
                return;
            }
            backwardSpeedMod = 0.6f;
            animator.SetBool("isBlocking", true);
            isBlocking = true;
        }
        else
        {
            backwardSpeedMod = 1;
            animator.SetBool("isBlocking", false);
            isBlocking = false;
        }

        if (!isGrounded && horizontal > 0 && playerNumber == 1)
        {
            pullback = 0.7f;
        } else
        {
            pullback = 1;
        }

        if (!isGrounded && horizontal < 0 && playerNumber == 2)
        {
            pullback = 1.4f;
        }
        else
        {
            pullback = 1;
        }

        if (isSprinting)
        {
            isBlocking = false;
            animator.SetBool("isBlocking", false);
        }

        #endregion SpeedMods

        prevPos = transform.position.z;

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

            if (playerNumber == 1)
            {
                if (horizontal > 0)
                {
                    speed = 15;
                }
                
                if (horizontal < 0)
                {
                    speed = 10;
                }
            } else if (playerNumber == 2) 
            {
                if (horizontal < 0)
                {
                    speed = 15;
                }

                if (horizontal > 0)
                {
                    speed = 10;
                }
            }

            runAccel = 1.07f;
        }
    }

    public void CancelSprint()
    {
        isSprinting = false;
        maxParaSpeed = 1;
        maxParaNegativeSpeed = -1;
        speed = 5;
        runAccel = 1;
    }

    private void Crouch(bool downpressed)
    {
        
        if (!isGrounded)
        {
            crouchSpeedMod = 1;
            return;
        }

        if (isBlocking && playerHealth.hitDefended)
        {
            playerHealth.crouchBlockHit = true;
            animator.SetBool("Hit", false);
            animator.Play("Base Layer.CrouchBlock");
            animator.SetBool("HitDefended", true);
            Actions.OnPlayerHit.Invoke(1);
            playerHealth.EndHit();
        }

        if (playerHealth.hit)
        {
            playerHealth.hitDefended = false;
            animator.SetBool("HitDefended", false);
            animator.Play("Base Layer.CrouchHit");
            Invoke("EndHit", 0.5f);
            Actions.OnPlayerHit.Invoke(1);
            return;
        }

        isCrouching = true;

        if (downpressed && isCrouching)
        {
            if (playerHealth.crouchBlockHit)
            {
                isCrouching = false;
                playerHealth.crouchBlockHit = false;
            } else
            {
                isCrouching = true;
                crouchSpeedMod = 0.8f;
                animator.SetBool("isCrouching", true);
                animator.Play("Base Layer.Crouch");
            }
        }
    }

    public void CancelCrouch()
    {
        isCrouching = false;
        crouchSpeedMod = 1f;
        animator.SetBool("isCrouching", false);
    }

    public void Dash(bool dashPressed)
    {
        if (!isGrounded)
        {
            print("airReturn");
            return;
        }

        if (isCrouching)
        {
            print("crouchReturn");
            return;
        }

        if (direction == 0 && playerNumber == 1)
        {
            return;
        }

        if (direction == 0 && playerNumber == 2)
        {
            return;
        }

        if (dashPressed)
        {
            if (playerNumber == 1)
            {

                if (dashPressed && direction == 1)
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

                if (dashPressed && direction == -1)
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
            else if (playerNumber == 2)
            {
          
                if (dashPressed && direction == -1)
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
                }

                if (dashPressed && direction == 1)
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

                }

            }

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

    public void Reverse()
    {
        if (player2.transform.position.z < player1.transform.position.z || player1.transform.position.z > player2.transform.position.z)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.z = -initialLocalScaleZ;
            transform.localScale = currentScale;
        } else
        {
            Vector3 currentScale = transform.localScale;
            currentScale.z = initialLocalScaleZ;
            transform.localScale = currentScale;
        }
    }

    private void AllowMovement()
    {
        allowMovement = true;
    }

    private Coroutine delayCoroutine;
    private void SlowMovement(int playerNumber)
    {
        if (this.playerNumber != playerNumber)
        {
            return;
        }

        hitStop = 0.1f;

        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
        }

        delayCoroutine = StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        if (isCrouching)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        hitStop = 1;
    }

    private void OnEnable()
    {
        Actions.OnCountdownEnd += AllowMovement;
        Actions.OnPlayerHit += SlowMovement;
    }

    private void OnDisable()
    {
        Actions.OnCountdownEnd -= AllowMovement;
        Actions.OnPlayerHit -= SlowMovement;
    }

}
