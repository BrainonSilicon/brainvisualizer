using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainButtonBeh : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;

    private bool buttonStatus = false;

    public void OnPress()
    {
        buttonStatus = !buttonStatus;

        object1.SetActive(buttonStatus);
        object2.SetActive(!buttonStatus);
    }
}
