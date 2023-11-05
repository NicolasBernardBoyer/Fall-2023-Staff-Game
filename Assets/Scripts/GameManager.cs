using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager instance = new GameManager(); // Singleton pattern
    [SerializeField] private int turn = 0;

    public static Action<int> onTurn;
    private bool gameHasEnded = false;

    public bool isPaused = false;

    private GameManager()
    {
        // Initialize game setup here (e.g., loading assets, setting up initial game state).
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    public void IncrementTurn()
    {
        turn++;
        onTurn?.Invoke(turn);
        Debug.Log("Turn: " + turn);
    }

    public void GameOver()
    {
        if (gameHasEnded == false)
        {
            PauseMovement();
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            UIManager.Instance.GameOver();
        }
        
    }

    private void PauseMovement()
    {
        isPaused = true;
    }

    public void Pause()
    {
        if (!isPaused)
        {
            PauseMovement();
            UIManager.Instance.Pause();
        }
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void Restart()
    {
        gameHasEnded = false;
        turn = 0;
        onTurn = null;
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        gameHasEnded = false;
        turn = 0;
        onTurn = null;
        Resume();
        SceneManager.LoadScene(0);
    }
}