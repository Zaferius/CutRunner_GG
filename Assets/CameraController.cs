using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera playCam;
    [Space(5)]
    public CinemachineFreeLook winCam;
    public float winCamRotationSpeed;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player").transform;
        playCam.Follow = player;
        winCam.Follow = player;
        winCam.LookAt = player;
    }


    private void OnEnable()
    {
        Actions.OnGameWin += WinCam;
    }

    private void OnDisable()
    {
        Actions.OnGameWin -= WinCam;
    }

    private void Update()
    {
        if (GameManager.i.gameState == GameManager.GameState.Win)
        {
            RotateWinCam();
        }
    }

    private void WinCam()
    {
        playCam.Priority = 0;
        winCam.Priority = 99;
    }

    private void RotateWinCam()
    {
        winCam.m_XAxis.Value += winCamRotationSpeed * Time.deltaTime;
    }
}
