using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {beforeStart, inPlay, nextRoundSetup}



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
