using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {beforeStart, inPlay, nextRoundSetup}

    public Movement Player1;
    public Movement Player2;
    public Combat combat1;
    public Combat combat2;
    public bool gameBegun;

    public GameStates currentGameState { get; private set; }


    public void SetGameState(GameStates gameState)
    {
        if (gameState == currentGameState)
        {
            return;
        }
        switch(gameState)
        {
            case GameStates.beforeStart:


                break;

            case GameStates.inPlay:

                gameBegun = true;

                break;

            case GameStates.nextRoundSetup:

                gameBegun = false;

                Player1.allowMovement = false;
                Player2.allowMovement = false;
                combat1.allowCombat = false;
                combat2.allowCombat = false;
                Invoke("Delay", 3.5f);

                break;

            default:
                Debug.LogError($"State not recognized.");

                break;
        }

        currentGameState = gameState;
        Actions.OnGameStateChange?.Invoke(gameState);
    }

    public void Delay()
    {
        Actions.OnNextRound.Invoke();
    }

}
