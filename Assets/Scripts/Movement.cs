using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region variables

    public bool debug;

    public GameObject player;
    public GameObject otherPlayer;
    public Animator animator;
    public RuntimeAnimatorController OGanim;
    public RuntimeAnimatorController reverseAnim;
    public AnimTriggers animTriggers;
    public Rigidbody rigidbody;
    public PlayerHealthManager playerHealth;
    public PauseMenu pauseMenuManager;
    public GameTimer gameTimer;
    public Combat combat;
    public GameController gameController;
    public int playerNumber;

    public Vector3 startingPosition;
    public Vector3 pausePos;

    public float speed;
    public int gradualIncrease = 5;
    private float animParaSpeed;
    private float acceleration;
    private bool holdingDown;
    private float maxParaSpeed = 1f;
    private float maxParaNegativeSpeed = -1f;
    private float runAccel = 1f;
    public bool reversed = false;
    public bool correctDir = true;

    public float dashCd;
    private float dashCdTimer;

    public Transform orientation;
    public float dashForce;
    public float dashDuration;

    public float jumpStrength;
    public bool isGrounded = true;
    public LayerMask playerLayer;

    public bool isSprinting;
    public bool isCrouching;
    public bool isBlocking;

    private float backwardSpeedMod;
    private float crouchSpeedMod = 1f;
    private float slightBoostMod = 1f;
    private float pullback = 1f;
    private float hitStop = 1f;

    private float mirrorPlayerFix = 0f;

    public AnimationClip[] randomHitAnimations;

    public bool allowMovement = false;

    private float prevPos;
    private float currentPos;
    private int direction;

    #endregion variables

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        DirectionCheck();
        prevPos = transform.position.z;
        currentPos = transform.position.z;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (pauseMenuManager.paused)
        {
            CancelSprint();
            CancelCrouch();
            Time.timeScale = 0;
            rigidbody.useGravity = false;
            return;
        }

        if (!pauseMenuManager.paused)
        {
            Time.timeScale = 1;
            rigidbody.useGravity = true;
        }

        if (allowMovement == false)
        {
            return;
        }

        #region reverseChecking

        if (otherPlayer.transform.position.z < transform.position.z && playerNumber == 1 && !reversed)
        {
            correctDir = false;
        }

        if (otherPlayer.transform.position.z > transform.position.z && playerNumber == 2 && !reversed)
        {
            correctDir = false;
        }

        if (otherPlayer.transform.position.z < transform.position.z && playerNumber == 2 && reversed)
        {
            correctDir = false;
        }

        if (otherPlayer.transform.position.z > transform.position.z && playerNumber == 1 && reversed)
        {
            correctDir = false;
        }

        if (otherPlayer.transform.position.z < transform.position.z && playerNumber == 2 && !reversed)
        {
            correctDir = true;
        }

        if (otherPlayer.transform.position.z > transform.position.z && playerNumber == 1 && !reversed)
        {
            correctDir = true;
        }

        if (otherPlayer.transform.position.z < transform.position.z && playerNumber == 1 && reversed)
        {
            correctDir = true;
        }

        if (otherPlayer.transform.position.z > transform.position.z && playerNumber == 2 && reversed)
        {
            correctDir = true;
        }

        if (!correctDir)
        {
            Reverse();
        }
        #endregion reverseChecking

        #region movement


        float horizontal = 0;
        bool shiftPressed = false;
        bool downPressed = false;
        bool dashPressed = false;
        bool upArrowPressed = false;
        bool reversePressed = false;

        bool cancelSprint = false;
        bool cancelCrouch = false;

        if (playerNumber == 2)
        {
            horizontal = Input.GetAxis("Horizontal");
            shiftPressed = Input.GetKey(KeyCode.RightShift);
            cancelSprint = Input.GetKeyUp(KeyCode.RightShift);
            downPressed = Input.GetKey(KeyCode.DownArrow);
            cancelCrouch = Input.GetKeyUp(KeyCode.DownArrow);
            dashPressed = Input.GetKeyDown(KeyCode.Keypad0);
            upArrowPressed = Input.GetKeyDown(KeyCode.UpArrow);
            //  reversePressed = Input.GetKeyDown(KeyCode.Return);
        }

        if (playerNumber == 1)
        {
            horizontal = Input.GetAxis("Horizontal2");
            shiftPressed = Input.GetKey(KeyCode.LeftShift);
            cancelSprint = Input.GetKeyUp(KeyCode.LeftShift);
            downPressed = Input.GetKey(KeyCode.S);
            cancelCrouch = Input.GetKeyUp(KeyCode.S);
            dashPressed = Input.GetKeyDown(KeyCode.E);
            upArrowPressed = Input.GetKeyDown(KeyCode.W);
            //reversePressed = Input.GetKeyDown(KeyCode.F);
        }

        GetIsGrounded();

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

        }
        else if (cancelSprint)
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
        }
        else if (cancelCrouch)
        {
            CancelCrouch();
        }

        if (prevPos == currentPos && !isCrouching && isGrounded)
        {
            StartCoroutine(GradualDecrease());
        }

        // if right is being pressed

        if (Input.anyKey && horizontal != 0f)
        {
            if (playerNumber == 1)
            {
                float posSpeed = Mathf.Abs(animParaSpeed);
                posSpeed *= -1;
                acceleration = posSpeed + Time.deltaTime + 0.1f;
                float PosHorizontal = Mathf.Abs(horizontal);
                posSpeed = PosHorizontal * acceleration * runAccel;
                holdingDown = true;
                animParaSpeed = posSpeed;
            }

            if (playerNumber == 2)
            {
                float posSpeed = -Mathf.Abs(animParaSpeed);
                posSpeed *= -1;
                acceleration = posSpeed - Time.deltaTime - 0.1f;
                float PosHorizontal = -Mathf.Abs(horizontal);
                posSpeed = PosHorizontal * acceleration * runAccel;
                holdingDown = true;
                animParaSpeed = posSpeed;
            }
        }

        // no keys are being pressed

        if (!Input.anyKey && holdingDown)
        {
            if (horizontal != 0f)
            {
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
                }
                else
                {
                    mirrorPlayerFix = 1f;
                }

                //float decimalValue = animParaSpeed -= mirrorPlayerFix;
                //animParaSpeed = animParaSpeed += decimalValue;
                //animParaSpeed -= Time.deltaTime;
                //print("decimalvalue = " + decimalValue);
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

                //float decimalValueN = animParaSpeed -= mirrorPlayerFix;
                //animParaSpeed = animParaSpeed += decimalValueN;
                //animParaSpeed += Time.deltaTime;
            }

            //float decimalValue = animParaSpeed -= mirrorPlayerFix;
            //animParaSpeed = animParaSpeed += decimalValue;
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
            if (reversed)
            {
                if (horizontal < 0 && !isSprinting && isGrounded)
                {
                    slightBoostMod = 1.4f;
                }

                if (horizontal > 0 || isSprinting || !isGrounded)
                {
                    slightBoostMod = 0.9f;
                }

                if (!isGrounded && horizontal < 0 && playerNumber == 2)
                {
                    pullback = 1.4f;
                }
                else
                {
                    pullback = 1;
                }

                if (horizontal > 0)
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

                if (horizontal < 0)
                {
                    backwardSpeedMod = 1;
                    animator.SetBool("isBlocking", false);
                    isBlocking = false;
                }
                return;
            }

            if (horizontal > 0 && !isSprinting && isGrounded)
            {
                slightBoostMod = 1.4f;
            }


            if (horizontal < 0 || isSprinting || !isGrounded)
            {
                slightBoostMod = 0.9f;
            }

            if (horizontal > 0)
            {
                backwardSpeedMod = 1;
                animator.SetBool("isBlocking", false);
                isBlocking = false;
            }

            if (!isGrounded && horizontal > 0 && playerNumber == 1)
            {
                pullback = 0.7f;
            }
            else
            {
                pullback = 1;
            }


            if (horizontal < 0)
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

        }
        else if (playerNumber == 2)
        {
            if (reversed)
            {
                if (horizontal > 0 && !isSprinting && isGrounded)
                {
                    slightBoostMod = 1.4f;
                }


                if (horizontal < 0 || isSprinting || !isGrounded)
                {
                    slightBoostMod = 0.9f;
                }

                if (horizontal > 0)
                {
                    backwardSpeedMod = 1;
                    animator.SetBool("isBlocking", false);
                    isBlocking = false;
                }

                if (!isGrounded && horizontal > 0 && playerNumber == 1)
                {
                    pullback = 0.7f;
                }
                else
                {
                    pullback = 1;
                }


                if (horizontal < 0)
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
                return;
            }

            if (horizontal < 0 && !isSprinting && isGrounded)
            {
                slightBoostMod = 1.4f;
            }

            if (horizontal > 0 || isSprinting || !isGrounded)
            {
                slightBoostMod = 0.9f;
            }

            if (!isGrounded && horizontal < 0 && playerNumber == 2)
            {
                pullback = 1.4f;
            }
            else
            {
                pullback = 1;
            }

            if (horizontal > 0)
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

            if (horizontal < 0)
            {
                backwardSpeedMod = 1;
                animator.SetBool("isBlocking", false);
                isBlocking = false;
            }
        }

        if (horizontal == 0)
        {
            backwardSpeedMod = 1;
            animator.SetBool("isBlocking", false);
            isBlocking = false;
        }


        if (isSprinting)
        {
            isBlocking = false;
            animator.SetBool("isBlocking", false);
        }

        #endregion SpeedMods

        prevPos = transform.position.z;
    }

    public void DirectionCheck()
    {
        if (playerNumber == 1)
        {
            direction = 1;
        }

        if (playerNumber == 2)
        {
            direction = -1;
        }
    }

    private bool GetIsGrounded()
    {
        int groundMask = ~playerLayer;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f, groundMask);
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.black);

        if (isGrounded)
        {
            backwardSpeedMod = 0.6f;
        }

        animator.SetBool("isGrounded", isGrounded);
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

            runAccel = 1.07f;

            if (playerNumber == 1)
            {
                if (reversed)
                {
                    if (horizontal < 0)
                    {
                        speed = 15;
                        animParaSpeed = 2;
                    }

                    if (horizontal > 0)
                    {
                        speed = 10;
                        animParaSpeed = -2;
                    }

                    return;
                }

                if (horizontal > 0)
                {
                    speed = 15;
                    animParaSpeed = 2;
                }

                if (horizontal < 0)
                {
                    speed = 10;
                    animParaSpeed = -2;

                }

            }
            else if (playerNumber == 2)
            {
                if (reversed)
                {
                    if (horizontal > 0)
                    {
                        speed = 15;
                        animParaSpeed = 2;
                    }

                    if (horizontal < 0)
                    {
                        speed = 10;
                        animParaSpeed = -2;
                    }

                    return;
                }

                if (horizontal < 0)
                {
                    speed = 15;
                    animParaSpeed = 2;
                }

                if (horizontal > 0)
                {
                    speed = 10;
                    animParaSpeed = -2;
                }
            }
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
            Actions.OnPlayerHit.Invoke(playerNumber);
            playerHealth.EndHit();
        }

        if (playerHealth.hit)
        {
            playerHealth.hitDefended = false;
            animator.SetBool("HitDefended", false);
            animator.Play("Base Layer.CrouchHit");
            Invoke("EndHit", 0.5f);
            Actions.OnPlayerHit.Invoke(playerNumber);
            return;
        }

        isCrouching = true;

        if (downpressed && isCrouching)
        {
            if (playerHealth.crouchBlockHit)
            {
                isCrouching = false;
                playerHealth.crouchBlockHit = false;
            }
            else
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

        if (direction == 0)
        {
            return;
        }

        if (isSprinting)
        {
            return;
        }

        if (dashPressed)
        {
            if (playerNumber == 1)
            {
                if (dashPressed && direction == 1 && reversed)
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

                    Vector3 forceToApply = orientation.forward * dashForce * direction;

                    rigidbody.AddForce(forceToApply, ForceMode.Impulse);
                    animator.SetBool("dashPressed", true);
                    animator.Play("Base Layer.DashBackward");
                    animator.SetBool("dashPressed", false);
                }

                if (dashPressed && direction == 1)
                {

                    if (dashCdTimer > 0)
                    {
                        print("timer is above 0");
                        return;
                    }
                    else dashCdTimer = dashCd;

                    Vector3 forceToApply = orientation.forward * dashForce * direction;

                    rigidbody.AddForce(forceToApply, ForceMode.Impulse);
                    animator.SetBool("dashPressed", true);
                    animator.Play("Base Layer.DashForward");
                    animator.SetBool("dashPressed", false);
                }

                if (dashPressed && direction == -1 && reversed)
                {

                    if (dashCdTimer > 0)
                    {
                        print("timer is above 0");
                        return;
                    }
                    else dashCdTimer = dashCd;

                    Vector3 forceToApply = orientation.forward * dashForce * direction;

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

                    Vector3 forceToApply = orientation.forward * dashForce * direction;

                    rigidbody.AddForce(forceToApply, ForceMode.Impulse);
                    animator.SetBool("dashPressed", true);
                    animator.Play("Base Layer.DashBackward");
                    animator.SetBool("dashPressed", false);
                }
            }
            else if (playerNumber == 2)
            {
                if (dashPressed && direction == -1 && reversed)
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

                    Vector3 forceToApply = orientation.forward * dashForce * direction;

                    rigidbody.AddForce(forceToApply, ForceMode.Impulse);
                    animator.SetBool("dashPressed", true);
                    animator.Play("Base Layer.DashBackward");
                    animator.SetBool("dashPressed", false);
                }

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
                    animator.SetBool("dashPressed", false);
                }

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
                    animator.SetBool("dashPressed", false);
                }

                if (dashPressed && direction == 1 && reversed)
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
                    animator.SetBool("dashPressed", false);
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

        animator.SetBool("isCrouching", false);
        animator.SetTrigger("Jump");
        //animator.Play("Jumping");
        backwardSpeedMod = 1f;
        rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        animator.SetBool("isGrounded", false);
    }

    IEnumerator GradualDecrease()
    {
        float posSpeed = animParaSpeed;

        while (posSpeed > 0f)
        {
            posSpeed -= 0.4f * Time.deltaTime * gradualIncrease;
            animParaSpeed = posSpeed;
            yield return new WaitForEndOfFrame();
        }

        while (posSpeed < 0f)
        {
            posSpeed += 0.4f * Time.deltaTime * gradualIncrease;
            animParaSpeed = posSpeed;
            yield return new WaitForEndOfFrame();
        }

    }

    public void Reverse()
    {
        // Player 1's initial rotation is 0
        // Player 2's initial rotation is 180
        if (correctDir)
        {
            return;
        }

        reversed = !reversed;
        correctDir = true;

        Vector3 currentRotation = transform.eulerAngles;

        direction *= -1;

        if (playerNumber == 1)
        {
            dashForce *= -1;
        }

        currentRotation.y = transform.localEulerAngles.y == 0 ? 180 : 0;
        transform.eulerAngles = currentRotation;

        if (animator.runtimeAnimatorController == OGanim)
        {
            animator.runtimeAnimatorController = reverseAnim;

            if (!isGrounded)
            {
                animator.Play("Base Layer.Falling");
            }

            return;
        }

        if (animator.runtimeAnimatorController == reverseAnim)
        {
            animator.runtimeAnimatorController = OGanim;

            if (!isGrounded)
            {
                animator.Play("Base Layer.Falling");
            }
        }

        // }
        //   else
        //    {
        // If player is to the right of the other player
        //     Quaternion currentRotation = transform.localRotation;

        //      if (playerNumber == 1)
        //      {
        //          currentRotation.y = initialLocalRotationY == 180 ? 0 : initialLocalRotationY;
        //      }
        //      else if (playerNumber == 2)
        //      {
        //          print("Reverse player 2");
        //          currentRotation.y = initialLocalRotationY == 0 ? 180 : initialLocalRotationY;
        //      }
        //          transform.localRotation = currentRotation;
        //}
    }

    private void AllowPlay()
    {
        allowMovement = true;
        combat.allowCombat = true;
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

    private void SetupNextRound()
    {

        gameTimer.timeUpText.gameObject.SetActive(false);

        if (playerNumber == 1)
        {
            Vector3 regularRo = transform.localEulerAngles;
            regularRo.y = 0;
            transform.localEulerAngles = regularRo;
            dashForce = 5;
        }


        if (playerNumber == 2)
        {
            Vector3 regularRo = transform.localEulerAngles;
            regularRo.y = 180;
            transform.localEulerAngles = regularRo;
        }

        animator.runtimeAnimatorController = OGanim;
        reversed = false;

        animator.SetBool("Knockout", false);
        animator.Play("Base Layer.Blend Tree");


        if (gameTimer.timeUp)
        {
            //return;
        }

    }

    public void PausePos()
    {
        pausePos = transform.position;
    }

    private void OnEnable()
    {
        Actions.OnCountdownEnd += AllowPlay;
        Actions.OnPlayerHit += SlowMovement;
        Actions.OnNextRound += SetupNextRound;
    }

    private void OnDisable()
    {
        Actions.OnCountdownEnd -= AllowPlay;
        Actions.OnPlayerHit -= SlowMovement;
        Actions.OnNextRound -= SetupNextRound;
    }

}