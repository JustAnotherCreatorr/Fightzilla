using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject otherPlayer;
    public GameController gc;
    public Movement playerMovement;
    public Movement otherPlayerMovement;
    public Slider healthBar;
    public Slider otherPlayerHealthBar;
    public AudioManager audioManager;
    public GameTimer gameTimer;
    public BugFix bf;
    public PlayerHealthUIManager OP;
    public CountdownTimer CT;
    public AnimTriggers playerAT;
    public AnimTriggers otherPlayerAT;


    public Animator animator;
    public bool hit;
    public bool hitDefended;
    public bool crouchBlockHit;
    public int losses = 0;


    public GameObject otherPlayerWinSymbol1;
    public GameObject otherPlayerWinSymbol2;
    public ParticleSystem hurtParticles;
    public ParticleSystem blockParticles;
    public ParticleSystem deathParticles;
    public Color fullHP;
    public Color nearFullHP;
    public Color halfHP;
    public Color lowHP;
    public Color critical;
    public Outline GEMplayerWon;
    public Color p1Win;
    public Color p2Win;
    public Color draw;
    public Color blank;
    public Text whichPlayerWin;
    public GameObject winningPlayer;
    public Text extraMessage;
    public GameObject GEM;
    public bool gameDone;
    public GameObject resume;
    public bool paused;
    public bool GEMActive;
    public bool settingUpNR;
    public GameObject PA;

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
        bool spacePressed = Input.GetKeyDown(KeyCode.P);
        Paused(spacePressed);

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

        if (playerMovement.isBlocking && hit)
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
        GEM.SetActive(false);
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
            GameEndMenu();
            audioManager.PlaySFX(audioManager.win);
            FindObjectOfType<GameController>().SetGameState(GameController.GameStates.gameOver);
        }
    }

    private void GameEndMenu()
    {
        GEMActive = true;

        if (losses == 2 && OP.losses == 2)
        {
            GEM.SetActive(true);
            PA.SetActive(true);
            resume.SetActive(false);
            whichPlayerWin.color = draw;
            whichPlayerWin.text = "DRAW!";
            GEMplayerWon.effectColor = draw;
            extraMessage.text = "The fight never ends!";
        }

        if (otherPlayer == winningPlayer && playerMovement.playerNumber == 1)
        {
            GEM.SetActive(true);
            PA.SetActive(true);
            resume.SetActive(false);
            whichPlayerWin.color = p2Win;
            whichPlayerWin.text = "Player 2 wins!";
            GEMplayerWon.effectColor = p2Win;
            extraMessage.text = "The fight never ends!";
        }

        if (otherPlayer == winningPlayer && playerMovement.playerNumber == 2)
        {
            GEM.SetActive(true);
            PA.SetActive(true);
            resume.SetActive(false);
            whichPlayerWin.color = p1Win;
            whichPlayerWin.text = "Player 1 wins!";
            GEMplayerWon.effectColor = p1Win;
            extraMessage.text = "The fight never ends!";
        }
    }

    public void Paused(bool spacePressed)
    {
        if (CT.end == false)
        {
            return;
        }

        if (GEMActive)
        {
            return;
        }

        if (!spacePressed)
        {
            return;
        }

        if (gameDone)
        {
            return;
        }

        if (settingUpNR)
        {
            return;
        }

        playerMovement.allowMovement = false;
        otherPlayerMovement.allowMovement = false;
        whichPlayerWin.color = blank;
        GEMplayerWon.effectColor = blank;
        paused = true;
        OP.paused = true;
        audioManager.PauseAudio();
        GEM.SetActive(true);
        PA.SetActive(false);
        whichPlayerWin.text = "Paused";
        extraMessage.text = "The fight never ends!";
        resume.SetActive(true);
    }

    public void Resume()
    {
        playerMovement.allowMovement = true;
        otherPlayerMovement.allowMovement = true;
        paused = false;
        OP.paused = false;
        audioManager.ResumeAudio();
        GEM.SetActive(false);
        resume.SetActive(false);
    }

}
