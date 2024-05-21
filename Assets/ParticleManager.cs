using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager i;
    
    public ParticleSystem starExplosion;
    
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
}
