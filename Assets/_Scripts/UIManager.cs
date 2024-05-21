using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    
    private void OnEnable()
    {
        Actions.OnStartGame += StartGame;
    }

    private void OnDisable()
    {
        Actions.OnStartGame -= StartGame;
    }

    
    void Start()
    {
        startButton.onClick.AddListener(GameManager.i.StartGame);
    }

    private void StartGame()
    {
        startButton.interactable = false;
    }
}
