using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictMovement : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(transform.position.x, 0, 0);
        transform.position = currentPos;

        currentPos.y = Mathf.Clamp(transform.position.y, 0, 4);
        transform.position = currentPos;

        float depth = Mathf.Abs(Camera.main.transform.position.x) + transform.position.x;

        Vector3 minPos = Camera.main.ViewportToWorldPoint(new Vector3 (0.1f, 0, depth));

        Vector3 maxPos = Camera.main.ViewportToWorldPoint(new Vector3 (0.9f, 0, depth));

        currentPos.z = Mathf.Clamp(transform.position.z, minPos.z, maxPos.z);
        transform.position = currentPos;

    }
}
