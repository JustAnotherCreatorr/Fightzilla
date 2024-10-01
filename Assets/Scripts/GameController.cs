using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {beforeStart, inPlay, nextRoundSetup}

    public Movement Player1;
    public Movement Player2;

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



                break;

            case GameStates.nextRoundSetup:

                Player1.allowMovement = false;
                Player2.allowMovement = false;
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
