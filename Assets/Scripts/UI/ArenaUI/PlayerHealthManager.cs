using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public GameObject player;
    public GameObject otherPlayer;
    public GameController gc;
    public Movement playerMovement;
    public Movement otherPlayerMovement;
    public AnimTriggers playerAT;
    public AnimTriggers otherPlayerAT;
    public PlayerHealthManager OP;

    public Slider healthBar;
    public Slider otherPlayerHealthBar;
    public Color fullHP;
    public Color nearFullHP;
    public Color halfHP;
    public Color lowHP;
    public Color critical;

    public AudioManager audioManager;
    public GameTimer gameTimer;
    public BugFix bf;
    public Animator animator;

    public bool hit;
    public bool hitDefended;
    public bool crouchBlockHit;
    public int losses = 0;
    public float lastHitStunDuration = 0.5f;

    public GameObject otherPlayerWinSymbol1;
    public GameObject otherPlayerWinSymbol2;

    public ParticleSystem hurtParticles;
    public ParticleSystem blockParticles;
    public ParticleSystem deathParticles;

    public GameObject winningPlayer;

    public GameEndMenuManager gameEndMenuManager;
    public Combat combat;

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

        if (playerAT.facingEnemy == false)
        {
            playerMovement.isBlocking = false;
        }

        if (hitDefended)
        {
            animator.SetBool("HitDefended", true);
            hit = false;
            animator.SetBool("Hit", false);
        }

        if (hit == false)
        {
            animator.SetBool("Hit", false);
            animator.SetBool("HitDefended", true);
        }

        if (crouchBlockHit)
        {
            EndHit();
        }
    }

    public void PlayerHurt(AttackSO attackSO)
    {
        bool isUnblockable = attackSO.attackName == "HPunch" || attackSO.attackName == "HKick";

        lastHitStunDuration = attackSO.hitStunDuration;

        if (playerMovement.isBlocking && hit && !isUnblockable)
        {
            animator.SetBool("Hit", false);
            hit = false;
            audioManager.PlaySFX(audioManager.block);
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
        audioManager.PlaySFX(audioManager.hurt);
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
        bf.timer.fix = false;
        deathParticles.Play();
        audioManager.PlaySFX(audioManager.KO);
        animator.SetBool("Knockout", true);
        animator.Play("Defeated");
        animator.SetBool("Knockout", false);
        losses += 1;
        LossUpdate();
        FindObjectOfType<GameController>().SetGameState(GameController.GameStates.nextRoundSetup);
    }

    public void HealthCompare()
    {

        if (healthBar.value < otherPlayerHealthBar.value)
        {
            losses += 1;
        }

        LossUpdate();
    }

    private void SetupNextRound()
    {
        if (losses == 0)
        {
            otherPlayerWinSymbol1.SetActive(false);
            otherPlayerWinSymbol2.SetActive(false);
        }

        healthBar.value = 1f;
        hurtParticles.Stop();
        deathParticles.Stop();
        blockParticles.Stop();
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

    private void LossUpdate()
    {
        if (losses >= 1)
        {
            otherPlayerWinSymbol1.SetActive(true);
        }

        if (losses >= 2)
        {
            otherPlayerWinSymbol2.SetActive(true);
            otherPlayer = winningPlayer;

            bool isDraw = losses == 2 && OP.losses == 2;
            bool otherPlayerIsWinner = otherPlayer == winningPlayer;
            gameEndMenuManager.ShowGameEndMenu(isDraw, otherPlayerIsWinner, playerMovement.playerNumber);

            audioManager.PlaySFX(audioManager.win);
            FindObjectOfType<GameController>().SetGameState(GameController.GameStates.gameOver);
        }
    }
}

