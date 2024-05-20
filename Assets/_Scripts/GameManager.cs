using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    
    private void InstanceMethod()
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
    
    public enum GameState
    {
        Play,
        Pause,
        Win,
        Lose,
        StartMenu,
    }
    public GameState gameState;

    private void Awake()
    {
        InstanceMethod();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void StartGame()
    {
        
    }
}
