using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimTriggers : MonoBehaviour
{

    public AttackReferences attackReferences;
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
        AttackSO currentAttack = null;

        foreach (AttackSO attackSO in attackReferences.attacks)
        {
            if (attackSO.attackName == attackName)
            {
                currentAttack = attackSO;
            }
        }

        float distance = Vector3.Distance(player.transform.position, otherPlayer.transform.position);

        distance = Mathf.Abs(distance);

        if (distance <= threshold)
        {
            otherPlayerHealth.hit = true;
            otherPlayerHealth.PlayerHurt(currentAttack);        
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, threshold);
    }
}
