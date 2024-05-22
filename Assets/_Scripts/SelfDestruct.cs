using System.Collections;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
   public float delay;

   private void Start()
   {
      StartCoroutine(Delay());
   }

   private IEnumerator Delay()
   {
      yield return new WaitForSeconds(delay);
      Destroy(gameObject);
   }
}
