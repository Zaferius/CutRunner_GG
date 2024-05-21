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
    [Header("Shake")]
     public float shakeIntensity = 1.2f;
     public float shakeDuration = 0.3f;

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
        Actions.OnPerfectTap += PerfectShake;
    }

    private void OnDisable()
    {
        Actions.OnGameWin -= WinCam;
        Actions.OnPerfectTap -= PerfectShake;
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
   
    
    public void ShakeCamera(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeDuration = duration;
        
        CinemachineBasicMultiChannelPerlin noise = playCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        float initialIntensity = shakeIntensity;
        float initialDuration = shakeDuration;
        
        noise.m_AmplitudeGain = shakeIntensity;
        noise.m_FrequencyGain = 1f;
        
        float decreaseFactor = 1.0f / shakeDuration;
        
        InvokeRepeating("DecreaseShake", 0, 0.01f);
    }

    
    void DecreaseShake()
    {
        CinemachineBasicMultiChannelPerlin noise = playCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        shakeIntensity -= Time.deltaTime * 5;
        noise.m_AmplitudeGain = shakeIntensity;
        
        if (shakeIntensity <= 0)
        {
            CancelInvoke("DecreaseShake");
            noise.m_AmplitudeGain = 0;
        }
    }

    private void PerfectShake()
    {
        ShakeCamera(3f, 0.1f);
    }
}
