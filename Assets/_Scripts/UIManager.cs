using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("GameWin")] 
    [SerializeField] private GameObject gameWinHolder;
    
    [Header("GameLose")] 
    [SerializeField] private GameObject gameLoseHolder;
    
    [Space(5)]
    [SerializeField] private Button startButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextButton;
    
    private void OnEnable()
    {
        Actions.OnStartGame += StartGame;
        Actions.OnGameLose += GameLose;
        Actions.OnGameWin += GameWin;
    }

    private void OnDisable()
    {
        Actions.OnStartGame -= StartGame;
        Actions.OnGameLose -= GameLose;
        Actions.OnGameWin -= GameWin;
    }
    private void Start()
    {
        startButton.onClick.AddListener(GameManager.i.StartGame);
        retryButton.onClick.AddListener(SceneReload);
        nextButton.onClick.AddListener(SceneReload);
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
    
    private void GameWin()
    {
        TimeManager.i.transform.DOMoveX(0, 2f).OnComplete(() =>
        {
            gameWinHolder.SetActive(true);
        });
    }

    private void SceneReload()
    {
        SceneManager.LoadScene(0);
    }
}
