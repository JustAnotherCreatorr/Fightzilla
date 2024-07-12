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

    public Animator animator;
    public int gradualIncrease = 5;
    private float speed;
    private float acceleration;
    private bool holdingDown;
    private float maxSpeed = 1;
    private float maxNegativeSpeed = -1;
    private float runAccel = 1;


    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        bool shiftPressed = Input.GetKey(KeyCode.RightShift);

        Sprint(shiftPressed);

        // if right is being pressed

        if (Input.anyKey && horizontal != 0f)
        {
            float posSpeed = Mathf.Abs(speed); 
            acceleration = posSpeed + Time.deltaTime;
            float PosHorizontal = Mathf.Abs(horizontal);
            posSpeed = PosHorizontal * acceleration * runAccel;
            print($"Acceleration: {acceleration}. Speed: {posSpeed}. Horizontal: {PosHorizontal}");
            holdingDown = true;
            speed = posSpeed;
        }

        // no keys are being pressed

        if (!Input.anyKey && holdingDown)
        {
            Debug.Log("A key was released");
            holdingDown = false;
            acceleration = 0f;
            StartCoroutine(GradualDecrease());
        }

        if (horizontal > 0)
        {
            speed = Mathf.Abs(speed);
        } 
        else if (horizontal < 0)
        {
            speed = -Mathf.Abs(speed);
        }

        speed = Mathf.Clamp(speed, maxNegativeSpeed, maxSpeed);

        animator.SetFloat("MoveSpeed", speed);
    }

    private void Sprint(bool shiftPressed)
    {
        if (shiftPressed)
        {
            maxSpeed = 2;
            maxNegativeSpeed = -2;
            runAccel = 1.07f;
        }
        else
        {
            maxSpeed = 1;
            maxNegativeSpeed = -1;
            runAccel = 1;
        }
    }

    IEnumerator GradualDecrease()
     {
        while (speed > 0f)
        {
            speed -= 0.1f * Time.deltaTime * gradualIncrease;
            Debug.Log(speed);
            yield return new WaitForEndOfFrame();
        }

        if (speed < 0f && !Input.GetKey(KeyCode.LeftArrow))
        {
            speed = 0f;
        }
     } 
}
