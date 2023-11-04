using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager
{
    private static GameManager instance = new GameManager(); // Singleton pattern
    [SerializeField] private int turn = 0;

    public static Action<int> onTurn;

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
}