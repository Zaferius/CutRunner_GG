using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private bool _canGo;
    public float testSpeed;
    
    private void OnEnable()
    {
        Actions.OnStartGame += Go;
        Actions.OnGameWin += PlayerGameWin;
    }

    private void OnDisable()
    {
        Actions.OnStartGame -= Go;
        Actions.OnGameWin -= PlayerGameWin;
    }
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_canGo && GameManager.i.gameState == GameManager.GameState.Play)
        {
            transform.Translate(transform.forward * (Time.deltaTime * testSpeed));
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
}
