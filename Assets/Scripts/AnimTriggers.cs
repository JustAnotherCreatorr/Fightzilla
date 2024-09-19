using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimTriggers : MonoBehaviour
{

    public Movement player;
    public Movement otherPlayer;
    public PlayerHealth playerHealth;
    public PlayerHealth otherPlayerHealth;
    public Animator animator;
    public float threshold;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void CheckDistance(string attackName)
    {
        float distance = Vector3.Distance(player.transform.position, otherPlayer.transform.position);

        distance = Mathf.Abs(distance);

        if (distance <= threshold)
        {
            otherPlayerHealth.PlayerHurt();        
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, threshold);
    }
}
