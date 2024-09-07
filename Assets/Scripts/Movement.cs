using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{



    #region variables

    public GameObject player;
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
    private float dashForce = 5f;
    public float dashDuration;

    public float jumpStrength;
    public bool isGrounded = true;

    public bool isSprinting;
    public bool isCrouching;
    public bool isBlocking;

    private float backwardSpeedMod;
    private float crouchSpeedMod;
    private float slightBoostMod;
    private float pullback;
    private float hitStop = 1f;

    private float mirrorPlayerFix = 0f;

    public AnimationClip[] randomHitAnimations;

    private bool allowMovement = false;
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

        if (playerNumber == 1)
        {
             horizontal = Input.GetAxis("Horizontal");
             shiftPressed = Input.GetKey(KeyCode.RightShift);
             downPressed = Input.GetKey(KeyCode.DownArrow);
             dashPressed = Input.GetKeyDown(KeyCode.Comma);
             upArrowPressed = Input.GetKeyDown(KeyCode.UpArrow);
        } else if (playerNumber == 2)
        {
            horizontal = Input.GetAxis("Horizontal2");
            shiftPressed = Input.GetKey(KeyCode.LeftShift);
            downPressed = Input.GetKey(KeyCode.S);
            dashPressed = Input.GetKeyDown(KeyCode.Q);
            upArrowPressed = Input.GetKeyDown(KeyCode.W);
        }

        GetIsGrounded();

        Sprint(shiftPressed, horizontal);

        Dash(dashPressed);

        Jump(upArrowPressed);

        Crouch(downPressed);

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
            backwardSpeedMod = 0;
            crouchSpeedMod = 0;
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
                print("crouchblockhit");
                isCrouching = false;
                playerHealth.crouchBlockHit = false;
            } else
            {
                print("breahc");
                isCrouching = true;
                crouchSpeedMod = 0.8f;
                animator.SetBool("isCrouching", true);
                animator.Play("Base Layer.Crouch");
            }
        }
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

        if (dashPressed && animParaSpeed > 0f)
        {
            if (dashCdTimer > 0)
            {
                print("timer is above 0");
                return;
            }
            else dashCdTimer = dashCd;

            Vector3 forceToApply = orientation.forward * dashForce;
            Debug.Log($"transform: {player.transform.position}");
            Debug.Log($"{forceToApply}");
            rigidbody.AddForce(forceToApply * 10, ForceMode.Impulse);
            print("forceApplied");
            animator.SetBool("dashPressed", true);
            animator.Play("Base Layer.DashForward");
            animator.SetBool("dashPressed", false);
            print("doneDash");
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
            Debug.Log($"transform: {player.transform.position}");
            Debug.Log($"{forceToApply}");
            rigidbody.AddForce(forceToApply, ForceMode.Impulse);
            print("forceApplied");
            animator.SetBool("dashPressed", true);
            animator.Play("Base Layer.DashBackward");
            animator.SetBool("dashPressed", false);
            print("doneDash");
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
