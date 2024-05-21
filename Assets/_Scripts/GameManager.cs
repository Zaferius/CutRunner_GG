using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public List<MovingPlatform> placedPlatforms = new List<MovingPlatform>();
    public MovingPlatform activePlatform;
    private Transform _previousPlatform;
    private int placedPlatformIndex;

    [Space(5)] 
    private float _currentZPos;
    public GameObject playPlatform;
    [Space(5)] 
    public int perfectTapCombo;
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
        placedPlatformIndex = -1;
        _currentZPos = 4;
    }

    public void StartGame()
    {
        Actions.OnStartGame();
    }
    
    void Update()
    {
        if (gameState != GameState.Play) return;

        if (placedPlatformIndex == -1 && player.transform.localPosition.z > 0.1f)
        {
            SpawnPlatform();
        }

        if (Input.GetMouseButtonDown(0) && activePlatform)
        {
           StopPlatform();
        }
    }

    private void StopPlatform()
    {
        CalculatePlacePos();

        activePlatform.StopPlatform();

        SpawnPlatform();
    }

    private void SpawnPlatform()
    {
        placedPlatformIndex++;
        _currentZPos += 2;
        var platformObj = Instantiate(playPlatform, new Vector3(0, 0, _currentZPos), Quaternion.identity);
        var platformSc = platformObj.GetComponent<MovingPlatform>();
        placedPlatforms.Add(platformSc);
        platformSc.StartPlatform();
      
    }

    private void CalculatePlacePos()
    {
        float diff;
        if (placedPlatformIndex > 0)
        {
            _previousPlatform = placedPlatforms[placedPlatformIndex - 1].transform;
            diff = _previousPlatform.localPosition.x - activePlatform.transform.localPosition.x;
        }
        else
        {
            _previousPlatform = GameObject.FindGameObjectWithTag("StartingPlatform").transform;
            diff = activePlatform.transform.localPosition.x - 0;
        }

        if (diff < 0)
        {
            diff *= -1;
        }

        if (diff <= placeTolerance)
        {
            Actions.OnPerfectTap();
            perfectTapCombo++;

            Instantiate(ParticleManager.i.starExplosion, activePlatform.transform.position, Quaternion.identity);
            SoundManager.instance.PlaySoundPerfectTap(SoundManager.instance.perfectTap);

            if (placedPlatformIndex == 0)
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
        else
        {
            perfectTapCombo = 0;
            float xOffset = activePlatform.transform.localPosition.x - _previousPlatform.transform.localPosition.x;
            CutAndDestroy(activePlatform.gameObject, xOffset);
        }
    }
    
 void CutAndDestroy(GameObject platform, float xOffset)
{
    float platformWidth = platform.transform.localScale.x;
    float newWidth = platformWidth - Mathf.Abs(xOffset);
    
    if (newWidth <= 0)
    {
        Destroy(platform);
        return;
    }
    
    Vector3 initialScale = platform.transform.localScale;
    Vector3 initialPosition = platform.transform.position;

    Vector3 newScale = new Vector3(newWidth, initialScale.y, initialScale.z);
    float halfInitialWidth = initialScale.x / 2f;
    float halfNewWidth = newScale.x / 2f;

    Vector3 newPosition;

   
    if (xOffset > 0)
    {
        newPosition = initialPosition + new Vector3(-halfInitialWidth + halfNewWidth, 0, 0);
    }
    else
    {
        newPosition = initialPosition + new Vector3(halfInitialWidth - halfNewWidth, 0, 0);
    }
    
    platform.transform.localScale = newScale;
    platform.transform.position = newPosition;
    playPlatform = platform;
    
    float cutWidth = Mathf.Abs(xOffset);
    Vector3 cutScale = new Vector3(cutWidth, initialScale.y, initialScale.z);
    Vector3 cutPosition;
    
    if (xOffset > 0)
    {
        cutPosition = initialPosition + new Vector3(halfInitialWidth - newWidth / 2f + cutWidth / 2f, 0, 0);
    }
    else
    {
        cutPosition = initialPosition - new Vector3(halfInitialWidth - newWidth / 2f + cutWidth / 2f, 0, 0);
    }
    
    GameObject cutPart = GameObject.CreatePrimitive(PrimitiveType.Cube);
    
    cutPart.GetComponent<MeshRenderer>().material.DOColor(Color.white,0).OnComplete(() =>
    {
        cutPart.GetComponent<MeshRenderer>().material.DOColor(Color.gray, 0.6f);
    });
    
    cutPart.layer = 7;
    cutPart.transform.localScale = cutScale;
    cutPart.transform.position = cutPosition;
    cutPart.AddComponent<Rigidbody>();
    var cutPartRb = cutPart.GetComponent<Rigidbody>();
    cutPartRb.AddTorque(new Vector3(Random.Range(-50,50),Random.Range(-75,75),Random.Range(-5,5)));

    var dir = 1;
    if (xOffset > 0)
    {
        dir = 1;
    }
    else
    {
        dir = -1;
    }
    
    cutPartRb.AddForce(platform.transform.right * (dir * 100), ForceMode.Force);
    
    Destroy(cutPart, 3f);
}
    
}
