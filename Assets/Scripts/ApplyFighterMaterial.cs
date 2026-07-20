using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFighterMaterial : MonoBehaviour
{
    public Renderer player1Renderer;
    public Renderer player2Renderer;

    // Start is called before the first frame update
    void Awake()
    {
        if (InitializeFight.Instance.player1Material != null)
        {
            player1Renderer.material = InitializeFight.Instance.player1Material;
        }

        if (InitializeFight.Instance.player2Material != null)
        {
            player2Renderer.material = InitializeFight.Instance.player2Material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
