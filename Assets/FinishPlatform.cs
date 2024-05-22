using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    [SerializeField] private Transform throneContainer;
    [SerializeField] private Transform throneHolder;
    [SerializeField] private Transform winPos;
    [SerializeField] private float _distanceToWin;

    private Transform player;

    private void Awake()
    {
        throneHolder.transform.localScale = new Vector3(1, 0, 1);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        if (GameManager.i.gameState == GameManager.GameState.Play)
        {
            winPos.transform.position = new Vector3(player.transform.position.x, winPos.transform.position.y, winPos.transform.position.z);
            _distanceToWin = Vector3.Distance(winPos.position, player.position);
            if (_distanceToWin < 0.2f)
            {
                PlayerToThrone();
            }
        }
        
    }

    private void PlayerToThrone()
    {
        GameManager.i.gameState = GameManager.GameState.Win;
        player.transform.DOLocalMove(throneContainer.transform.position, 1f)
            .OnComplete(() =>
            {
                Actions.OnGameWin();
                player.transform.DOLocalMoveY(4, 1).SetEase(Ease.OutQuad);
                throneHolder.DOScaleY(2, 1).SetEase(Ease.OutQuad);
            });
      
    }
}
