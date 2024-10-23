using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {beforeStart, inPlay, nextRoundSetup, gameOver}

    public Movement Player1;
    public Movement Player2;
    public Combat combat1;
    public Combat combat2;
    public PlayerHealthUIManager ph1;
    public PlayerHealthUIManager ph2;
    public GameTimer gameTimer;

    public GameStates currentGameState { get; private set; }

    private void Update()
    {

        if (ph1.losses == 2 || ph2.losses == 2)
        {
            SetGameState(GameStates.gameOver);
            return;
        }

        if (currentGameState != GameStates.inPlay)
        {
            Player1.isGrounded = true;
            Player1.animator.SetBool("isGrounded", true);
            Player2.isGrounded = true;
            Player2.animator.SetBool("isGrounded", true);
        }
    }

    public void SetGameState(GameStates gameState)
    {
        if (gameState == currentGameState)
        {
            return;
        }


        switch(gameState)

        {
            case GameStates.beforeStart:

                ClampCheck();

                break;

            case GameStates.inPlay:

                break;

            case GameStates.nextRoundSetup:

                Player1.allowMovement = false;
                Player2.allowMovement = false;
                combat1.allowCombat = false;
                combat2.allowCombat = false;

                if (ph1.losses == 2 || ph2.losses == 2)
                {
                    SetGameState(GameStates.gameOver);
                    return;
                }


                Invoke("Delay", 3.5f);

                break;

            case GameStates.gameOver:

                Actions.OnGameOver.Invoke();

                break;

            default:

                break;
        }

        currentGameState = gameState;
        Actions.OnGameStateChange?.Invoke(gameState);
    }

    public void Delay()
    {
        gameTimer.timeUpText.gameObject.SetActive(false);
        SetGameState(GameStates.beforeStart);
        Actions.OnNextRound.Invoke();
    }


    public void ClampCheck()
    {
        Player1.transform.position = Player1.startingPosition;
        Player2.transform.position = Player2.startingPosition;
    }
}
