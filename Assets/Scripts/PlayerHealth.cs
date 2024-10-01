using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject player;
    public GameObject otherPlayer;
    public Movement playerMovement;
    public Movement otherPlayerMovement;
    public Slider healthBar;
    public Slider otherPlayerHealthBar;
    public Animator animator;
    public bool hit;
    public bool hitDefended;
    public bool crouchBlockHit;
    [SerializeField] private ParticleSystem hurtParticles;
    [SerializeField] private ParticleSystem blockParticles;
    [SerializeField] private ParticleSystem deathParticles;
    public Color fullHP;
    public Color nearFullHP;
    public Color halfHP;
    public Color lowHP;
    public Color critical;


    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("Hit", false);
        hurtParticles.Stop();
        deathParticles.Stop();
        blockParticles.Stop();
    }

    // Update is called once per frame
    void Update()
    {

        CheckColor();

        if (hitDefended)
        {
            animator.SetBool("HitDefended", true);
            hit = false;
            animator.SetBool("Hit", false);
        }    

        if (crouchBlockHit)
        {
            EndHit();
        }
    }

    public void PlayerHurt(AttackSO attackSO)
    {

        if (playerMovement.isBlocking && hit)
        {
            animator.SetBool("Hit", false);
            hit = false;
            animator.Play("Base Layer.Block");
            blockParticles.Play();
            hitDefended = true;
            animator.SetBool("HitDefended", true);
            Actions.OnPlayerHit.Invoke(playerMovement.playerNumber);
            Invoke("EndHit", 0.5f);
            return;
        }

        hitDefended = false;
        animator.SetBool("HitDefended", false);
        animator.SetFloat("HitAnimation", Random.Range(0, 4));
        hurtParticles.Play();
        animator.Play("Base Layer.Hit");
        healthBar.value -= attackSO.damage;
        Invoke("EndHit", 0.5f);
        Actions.OnPlayerHit.Invoke(playerMovement.playerNumber);

        if (healthBar.value == 0)
        {
            HealthRunOut();
        }

    }
    public void EndHit()
    {
        hit = false;
        animator.SetBool("Hit", false);
        Invoke("GuardDown", 0.5f);
    }

    public void GuardDown()
    {
        hitDefended = false;
        animator.SetBool("HitDefended", false);
    }


    public void HealthRunOut()
    {
        deathParticles.Play();
        animator.SetBool("Knockout", true);
        animator.Play("Defeated");
        animator.SetBool("Knockout", false);
        print("healthGone");
        FindObjectOfType<GameController>().SetGameState(GameController.GameStates.nextRoundSetup);
    }

    private void SetupNextRound()
    {
        healthBar.value = 1f;
        healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = fullHP;
    }

    private void OnEnable()
    {
        Actions.OnNextRound += SetupNextRound;
    }

    private void OnDisable()
    {
        Actions.OnNextRound -= SetupNextRound;
    }

    private void CheckColor()
    {
        if (healthBar.value <= 0.75f && healthBar.value >= 0.5f)
        {
            healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = nearFullHP;
        }

        if (healthBar.value <= 0.50f && healthBar.value >= 0.3f)
        {
            healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = halfHP;
        }

        if (healthBar.value <= 0.25f && healthBar.value >= 0.1f)
        {
            healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = lowHP;
        }

        if (healthBar.value <= 0.1f)
        {
            healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = critical;
        }
    }

}
