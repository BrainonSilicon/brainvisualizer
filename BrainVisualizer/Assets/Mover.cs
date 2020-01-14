using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 0.3f;
    public ParticleSystem ps;
    public Color color1;
    public Color color2;
    ParticleSystem.MinMaxGradient origGrad;
    Vector3 direction = new Vector3(1, 0, 0);
    ParticleSystem.ColorOverLifetimeModule col;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        origGrad = ps.colorOverLifetime.color;
        col = ps.colorOverLifetime;
        col.enabled = true;
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
        Debug.Log("hit collider " + other.name);
        transform.localScale *= 1.3f;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
                                              new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        col.color = grad;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit collider " + other.name);
        transform.localScale /= 1.3f;
        col.color = origGrad;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enter");
        transform.localScale = transform.localScale * 1.3f;

    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("stay");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit");
        transform.localScale = transform.localScale / 1.3f;

    }
}
