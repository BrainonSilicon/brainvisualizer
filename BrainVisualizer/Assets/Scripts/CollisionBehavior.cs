using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBehavior : MonoBehaviour
{
    float speed = 0.3f;
    public ParticleSystem[] psArray;
    public Color color1;
    public Color color2;
    ParticleSystem.MinMaxGradient origGrad;
    Vector3 direction = new Vector3(1, 0, 0);
    ParticleSystem.ColorOverLifetimeModule [] colArray;
    Gradient grad = new Gradient();
    public string targetName;

    // Start is called before the first frame update
    void Start()
    {
        origGrad = psArray[0].colorOverLifetime.color;
        colArray = new ParticleSystem.ColorOverLifetimeModule[psArray.Length];
        for (int i=0; i<colArray.Length; i++)
        {
            colArray[i] = psArray[i].colorOverLifetime;
            colArray[i].enabled = true;
        }
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
                                             new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
    }

    // Update is called once per frame
    void Update()
    {
        //if (name == targetName)
        //{
        //    transform.position += direction * speed * Time.deltaTime;
        //    if (transform.position.x > .5f || transform.position.x < -.5f)
        //    {
        //        direction *= -1;
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + "  " + other.name);
        if (other.name == "spotLightCone_highPoly")
        {
            for (int i = 0; i < colArray.Length; i++)
            {
                colArray[i].color = grad;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "spotLightCone_highPoly")
        {
            for (int i = 0; i < colArray.Length; i++)
            {
                colArray[i].color = origGrad;
            }
        }
    }
}
