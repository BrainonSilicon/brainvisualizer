using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recenter : MonoBehaviour
{
    public GameObject baseObject;
    public float distance;

    public void changePosition()
    {
        transform.position = baseObject.transform.position + baseObject.transform.forward * distance;
    }
}
