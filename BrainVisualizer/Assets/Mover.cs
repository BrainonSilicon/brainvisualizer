using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var currentPos = transform.position;
        currentPos.x += speed * Time.deltaTime;

        if (currentPos.x > 0.5f || currentPos.x < -0.5f)
        {
            speed *= -1;
        }
    }
}
