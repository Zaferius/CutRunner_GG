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
    [SerializeField] private int placedPlatformIndex;

    [Space(5)] 
    public LayerMask placedPlatformLayer;
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
    
    private void OnEnable()
    {
        Actions.OnGameLose += GameLose;
    }

    private void OnDisable()
    {
        Actions.OnGameLose -= GameLose;
    }

    public void StartGame()
    {
        Actions.OnStartGame();
    }

    private void Update()
    {
        if (gameState != GameState.Play) return;
        PlayerPosChecker();
        
        if (placedPlatformIndex >= Level.i.lvlScriptable.platformAmount) return;

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
        activePlatform = null;

        SpawnPlatform();
      
    }

    private void SpawnPlatform()
    {
        if (placedPlatformIndex < Level.i.lvlScriptable.platformAmount - 1)
        {
            placedPlatformIndex++;
            _currentZPos += 2;
            var platformObj = Instantiate(playPlatform, new Vector3(0, 0, _currentZPos), Quaternion.identity);
            platformObj.name = "Platform";
            platformObj.transform.parent = Level.i.transform;
            var platformSc = platformObj.GetComponent<MovingPlatform>();
            placedPlatforms.Add(platformSc);
            platformSc.StartPlatform();

            if (placedPlatforms.Count == Level.i.lvlScriptable.platformAmount - 2)
            {
                var finishPlatform = Instantiate(Level.i.lvlScriptable.finishPlatformPrefab, new Vector3(0, -15, _currentZPos + 10), Quaternion.identity);
                finishPlatform.transform.DOLocalMoveY(0, 1.75f).SetEase(Ease.OutQuad);
            }
        }
       
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
            if (perfectTapCombo > 10)
            {
                perfectTapCombo = 10;
            }

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
            SoundManager.instance.PlaySoundRandomPitch(SoundManager.instance.sad, 0.9f,1.2f);
            var xOffset = activePlatform.transform.localPosition.x - _previousPlatform.transform.localPosition.x;
            CutAndDestroy(activePlatform.gameObject, xOffset);
        }
    }

    private void CutAndDestroy(GameObject platform, float xOffset)
    {
        
       var platformWidth = platform.transform.localScale.x;
       var newWidth = platformWidth - Mathf.Abs(xOffset);
    
       if (newWidth <= 0)
       {
           MissPlacePlatform(platform.GetComponent<MovingPlatform>());
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
       platform.gameObject.layer = 9;
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
    
       cutPartRb.AddForce(platform.transform.right * (dir * 200), ForceMode.Force);
    
       TimeManager.i.transform.DOMoveX(0, 2f).OnComplete(() =>
       {
           cutPart.transform.DOScale(0,0.2f).SetEase(Ease.InBack).OnComplete(() =>
           {
               Destroy(cutPart);
           });
       });
      
    }

    private void MissPlacePlatform(MovingPlatform platform)
    {
        placedPlatforms.Remove(platform);
        platform.gameObject.layer = 7;
        placedPlatformIndex--;
        _currentZPos -= 2;
        platform.gameObject.AddComponent<Rigidbody>();
        var platformRb = platform.gameObject.GetComponent<Rigidbody>();
        platformRb.AddTorque(new Vector3(Random.Range(-50,50),Random.Range(-75,75),Random.Range(-5,5)));
        platform.GetComponent<MeshRenderer>().material.DOColor(Color.white,0).OnComplete(() =>
        {
            platform.GetComponent<MeshRenderer>().material.DOColor(Color.gray, 0.6f);
        });
        
        TimeManager.i.transform.DOMoveX(0, 2f).OnComplete(() =>
        {
            platform.transform.DOScale(0,0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(platform.gameObject);
            });
        });
    }

    private void PlayerPosChecker()
    {
        var rayCount = 20;
        var raySpacing = 0.1f;

        var playerPos = player.transform.position;
        var originPos = new Vector3(playerPos.x - ((rayCount - 1) * raySpacing * 0.5f), playerPos.y + 2, playerPos.z);

        for (var z = 0; z < rayCount; z++)
        {
            Vector3 rayOrigin = originPos + player.transform.right * (raySpacing * z) + player.transform.forward * 1.5f;

            Debug.DrawRay(rayOrigin, Vector3.down * 30, Color.red);
            
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 30, placedPlatformLayer))
            {
                var closestPlatform = hit.collider.gameObject;
                var platformCenter = closestPlatform.transform.position;
                var averagePoint = Vector3.Lerp(playerPos, platformCenter, 0.3f);
                var newPosition = playerPos;
                newPosition.x = averagePoint.x;
                
                player.transform.position = newPosition;
                break;
            }
        }
    }

    private void GameLose()
    {
        Instantiate(ParticleManager.i.starExplosion, player.transform.position, Quaternion.identity);
        Destroy(player.gameObject);
        gameState = GameState.Lose;
    }
    
}
