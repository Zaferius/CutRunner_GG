using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private bool _canGo;
    public float speed;
    private static readonly int RunMultiplier = Animator.StringToHash("runMultiplier");

    private void OnEnable()
    {
        Actions.OnStartGame += Go;
        Actions.OnGameWin += PlayerGameWin;
        Actions.OnWinPos += WinPosAnimSpeedChanger;
    }

    private void OnDisable()
    {
        Actions.OnStartGame -= Go;
        Actions.OnGameWin -= PlayerGameWin;
        Actions.OnWinPos -= WinPosAnimSpeedChanger;
    }
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    

    private void FixedUpdate()
    {
        if (_canGo && GameManager.i.gameState == GameManager.GameState.Play)
        {
            transform.Translate(transform.forward * (Time.deltaTime * speed));
        }
    }

    private void Go()
    {
        _canGo = true;
        animator.SetTrigger("run");
    }

    private void PlayerGameWin()
    {
        rb.useGravity = false;
        animator.SetTrigger("dance");
        _canGo = false;
    }

    private void WinPosAnimSpeedChanger()
    {
        animator.SetFloat(RunMultiplier, 2);
    }
    
}
