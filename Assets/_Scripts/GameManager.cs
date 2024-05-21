using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    public enum GameState
    {
        Play,
        Pause,
        Win,
        Lose,
        StartMenu,
    }
    public GameState gameState;

    private Player player;
    
    private void Awake()
    {
        if (i)
        {
            Destroy(this);
        }
        else
        {
            i = this;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    void Update()
    {
        
    }

    public void StartGame()
    {
        Actions.OnStartGame();
    }
}
