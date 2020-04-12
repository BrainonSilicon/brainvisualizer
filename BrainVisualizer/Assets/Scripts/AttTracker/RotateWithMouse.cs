using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]

public class RotateWithMouse : MonoBehaviour
{


    public float RotationSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate((Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime), (Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime), 0, Space.World);
    }


}