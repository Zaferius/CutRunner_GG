using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("GameWin")] 
    public GameObject gameWinHolder;
    
    [Header("GameLose")] 
    public GameObject gameLoseHolder;
    
    [Space(5)]
    public Button startButton;
    public Button retryButton;
    
    private void OnEnable()
    {
        Actions.OnStartGame += StartGame;
        Actions.OnGameLose += GameLose;
    }

    private void OnDisable()
    {
        Actions.OnStartGame -= StartGame;
        Actions.OnGameLose -= GameLose;
    }

    
    void Start()
    {
        startButton.onClick.AddListener(GameManager.i.StartGame);
        retryButton.onClick.AddListener(SceneReload);
    }

    private void StartGame()
    {
        gameLoseHolder.SetActive(false);
        gameWinHolder.SetActive(false);
        startButton.interactable = false;
    }

    private void GameLose()
    {
        gameLoseHolder.SetActive(true);
    }

    private void SceneReload()
    {
        SceneManager.LoadScene(0);
    }
}
