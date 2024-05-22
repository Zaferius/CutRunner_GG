using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingPlatform : MonoBehaviour
{
   public enum PlatformType
   {
       RightMover,
       LeftMover,
       StartingPlatform
   }

   [SerializeField] private PlatformType type;

   [SerializeField] private bool isMoving;
   public float movingSpeed;

   private Vector3 _defaultScale;

   private void Awake()
   {
       _defaultScale = transform.localScale;
   }

   private void Start()
   {
       SetPlatform();
   }
   
    void Update()
    {
        if (isMoving && GameManager.i.gameState == GameManager.GameState.Play)
        {
            if (type == PlatformType.RightMover)
            {
                transform.Translate(transform.right * (Time.deltaTime * movingSpeed));
            }
            else
            {
                transform.Translate(-transform.right * (Time.deltaTime * movingSpeed));
            }
        }
    }

    private void SetPlatform()
    {
        var randomInt = Random.Range(0, 2);
        type = randomInt == 0 ? PlatformType.RightMover : PlatformType.LeftMover;
        name = type + " " + "Platform";

        if (type == PlatformType.RightMover)
        {
            transform.localPosition = new Vector3(transform.localPosition.x - 5, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x + 4, transform.localPosition.y, transform.localPosition.z);
        }
        
        Instantiate(ParticleManager.i.whiteStarExplosion, transform.position, Quaternion.identity);
        
    }

    public void StartPlatform()
    {
        isMoving = true;
        transform.DOScale(0, 0);
        transform.DOScale(_defaultScale, 0.35f).SetEase(Ease.OutBack);
        GetComponent<MeshRenderer>().material.DOColor(ColorManager.i.NextColor(), 0.2f);
        GameManager.i.activePlatform = this;
    }

    public void StopPlatform()
    {
        isMoving = false;
        gameObject.layer = 9;
    }
}
