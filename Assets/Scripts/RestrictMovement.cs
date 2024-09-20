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

        currentPos.z = Mathf.Clamp(transform.position.z, -9, 16.52f);
        transform.position = currentPos;

    }
}
