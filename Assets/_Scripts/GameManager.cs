using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public List<MovingPlatform> movingPlatforms = new List<MovingPlatform>();
    public MovingPlatform activePlatform;
    private Transform _previousPlatform;
    [SerializeField] private int index;

    [Space(5)]
    [Range(0f, 1f)]
    public float placeTolerance;
    
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
        Application.targetFrameRate = 120;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        index = -1;
    }
    
    public void StartGame()
    {
        Actions.OnStartGame();
    }
    
    void Update()
    {
        if (gameState != GameState.Play) return;

        if (index == -1 && player.transform.localPosition.z > 0.1f)
        {
            ActivateNextPlatform();
        }

        if (Input.GetMouseButtonDown(0) && activePlatform)
        {
           StopPlatform();
        }
    }

    private void ActivateNextPlatform()
    {

        if (index < movingPlatforms.Count - 1)
        {
            index++;
            activePlatform = movingPlatforms[index];
            activePlatform.StartPlatform();
        }
        else
        {
            
        }
        
    }

    private void StopPlatform()
    {
        CalculatePlacePos();

        activePlatform.StopPlatform();

        ActivateNextPlatform();
    }

    private void CalculatePlacePos()
    {
        float diff;
        if (index > 0)
        {
            _previousPlatform = movingPlatforms[index - 1].transform;
            diff = _previousPlatform.localPosition.x - activePlatform.transform.localPosition.x;
        }
        else
        {
            _previousPlatform = GameObject.FindWithTag("StartingPlatform").transform;
            diff = activePlatform.transform.localPosition.x - 0;
        }

        if (diff < 0)
        {
            diff *= -1;
        }
        
        print("Diff is : " + " " + diff);
        
        if (diff <= placeTolerance)
        {
            /*Actions.OnPerfectTap();*/
            print("PERFECT!");
            activePlatform.transform.GetChild(0).GetComponent<MeshRenderer>().material.DOColor(Color.green, 0.1f);

            if (index == 0)
            {
                activePlatform.transform.DOLocalMove(_previousPlatform.localPosition + new Vector3(0, 0, 6f), 0.1f)
                    .SetEase(Ease.OutBack);
            }
            else
            {
                activePlatform.transform.DOLocalMove(_previousPlatform.localPosition + new Vector3(0, 0, 2), 0.1f)
                    .SetEase(Ease.OutBack);
            }
        }
    }
    
}
