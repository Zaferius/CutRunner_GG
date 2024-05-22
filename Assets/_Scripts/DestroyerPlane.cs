using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerPlane : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         Actions.OnGameLose();
      }
   }
}
