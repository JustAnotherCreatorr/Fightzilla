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

    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetFloat("MoveSpeed", 1f);
            }
            else
            {
                animator.SetFloat("MoveSpeed", horizontal);
            }
        } 

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StartCoroutine(GradualDecrease(horizontal));
        }
    }

    IEnumerator GradualDecrease(float horizontal)
    {

        float nhorizontal = horizontal - 0.01f;
        animator.SetFloat("MoveSpeed", nhorizontal);
        Debug.Log(horizontal);
        yield return new WaitForSeconds(.1f);
        
    }
}
