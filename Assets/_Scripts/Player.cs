using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private bool _canGo;
    
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
            rb.AddForce(transform.forward * 50, ForceMode.Force);
        }
    }

    private void Go()
    {
        _canGo = true;
        animator.SetTrigger("run");
    }
}
