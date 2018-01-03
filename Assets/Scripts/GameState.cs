using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

// Basic gamestate controller.
public class GameState : MonoBehaviour
{

    // I like unityevents when playing with UI, only because they are exposed in the inspector.
    // One and only reason why anyone should ever use them otherwise. (2.5x more costly than .NET events/delegates whatever)
    public UnityEvent OnNewGame;
    public UnityEvent OnGameOver;

    private void Awake()
    {
        // Initialization

    }

    public void StartGame()
    {
        if (OnNewGame != null)
            OnNewGame.Invoke();
    }

    public void GameOver()
    {
        if (OnGameOver != null)
            OnGameOver.Invoke();
    }
}
