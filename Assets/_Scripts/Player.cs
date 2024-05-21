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
    }

    private void OnDisable()
    {
        Actions.OnStartGame -= Go;
    }
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_canGo)
        {
            transform.Translate(transform.forward * (Time.deltaTime * testSpeed));
        }
    }

    private void Go()
    {
        _canGo = true;
        animator.SetTrigger("run");
    }
}
