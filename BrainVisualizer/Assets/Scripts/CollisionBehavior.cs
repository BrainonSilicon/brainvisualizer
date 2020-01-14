using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBehavior : MonoBehaviour
{
    float speed = 0.3f;
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public ParticleSystem ps3;
    public ParticleSystem ps4;
    public Color color1;
    public Color color2;
    ParticleSystem.MinMaxGradient origGrad;
    Vector3 direction = new Vector3(1, 0, 0);
    ParticleSystem.ColorOverLifetimeModule col1;
    ParticleSystem.ColorOverLifetimeModule col2;
    ParticleSystem.ColorOverLifetimeModule col3;
    ParticleSystem.ColorOverLifetimeModule col4;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        origGrad = ps1.colorOverLifetime.color;
        col1 = ps1.colorOverLifetime;
        col1.enabled = true;
        col2 = ps2.colorOverLifetime;
        col2.enabled = true;
        col3 = ps3.colorOverLifetime;
        col3.enabled = true;
        col4 = ps4.colorOverLifetime;
        col4.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += direction * speed * Time.deltaTime;
        //if (transform.position.x > .5f || transform.position.x < -.5f)
        //{
        //    direction *= -1;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //        transform.localScale = transform.localScale * 2;
        Debug.Log(name + " hit collider " + other.name);
   //     transform.localScale *= 1.3f;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
                                              new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        col1.color = grad;
        col2.color = grad;
        col3.color = grad;
        col4.color = grad;
    }

    private void OnTriggerExit(Collider other)
    {
     //   Debug.Log("exit collider " + other.name);
    //    transform.localScale /= 1.3f;
        col1.color = origGrad;
        col2.color = origGrad;
        col3.color = origGrad;
        col4.color = origGrad;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("enter");
    //    transform.localScale = transform.localScale * 1.3f;

    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log("stay");
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("Exit");
    //    transform.localScale = transform.localScale / 1.3f;

    //}
}
