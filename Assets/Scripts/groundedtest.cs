using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundedtest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool hit = Physics.Raycast(transform.position, Vector3.down, 1f, LayerMask.GetMask("Floor"));
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.black);
        print(hit);
    }
}
