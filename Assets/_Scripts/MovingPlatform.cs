using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private bool isMoving;
   private Vector3 _defaultScale;
   
   
  
   public int moveDirection = 1;
   public float totalMoveTime = 5.0f;
   public float movingSpeed = 1.0f;
   public float elapsedTime = 0.0f;
   

   private void Awake()
   {
       _defaultScale = transform.localScale;
   }

   private void Start()
   {
       SetPlatform();
       elapsedTime = 0;
   }
   
    void Update()
    {
        if (isMoving && GameManager.i.gameState == GameManager.GameState.Play)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= totalMoveTime)
            {
                moveDirection *= -1;
                elapsedTime = 0.0f;
            }
        
            if (moveDirection == 1)
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
        if (randomInt == 0)
        {
            moveDirection = 1;
        }
        else
        {
            moveDirection = -1;
        }

        if (moveDirection == 1)
        {
            transform.localPosition = new Vector3(-3.5f, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(3.5f, transform.localPosition.y, transform.localPosition.z);
        }
        
        Instantiate(ParticleManager.i.whiteStarExplosion, transform.position, Quaternion.identity);
        
    }

    public void StartPlatform()
    {
        gameObject.layer = 8;
        isMoving = true;
        transform.DOScale(0, 0);
        transform.DOScale(_defaultScale, 0.35f).SetEase(Ease.OutBack);
        GetComponent<MeshRenderer>().material.DOColor(ColorManager.i.NextColor(), 0.2f);
        GameManager.i.activePlatform = this;
        
    }

    public void StopPlatform()
    {
        isMoving = false;
        /*gameObject.layer = 9;*/
    }
}
