using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateToward : MonoBehaviour
{
    public GameObject target;
    public float rotationSpeed;


    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        //      this.transform.LookAt(target.transform);

        //find the vector pointing from our position to the target
        _direction = (transform.position - target.transform.position).normalized;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }
}
