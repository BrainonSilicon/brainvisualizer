using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTapBeh : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    private bool state = false;

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            object3.SetActive(state);
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.name == this.name)
                {
                    object1.SetActive(state);
                    object2.SetActive(!state);
                    state = !state;
                }
            }
        }
    }
}
