using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
 //   public UnityEngine.ParticleSystem particleSys;
 //   public UnityEngine.Color color;
 //   public UnityEngine.ParticleSystem.MinMaxGradient color1;

    public void Start()
    {
   //     var c = particleSys.colorOverLifetime;
  //      c.color = color;
    }

    private void OnCollisionExit(Collision collision)
    {
        transform.localScale /= 1.3f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.localScale *= 1.3f;
    }
}
