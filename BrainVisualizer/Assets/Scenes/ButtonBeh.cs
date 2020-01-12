using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBeh : MonoBehaviour
{
    public UnityEngine.UI.Text txt;
    bool buttonStatus;

    // Start is called before the first frame update
    void Start()
    {
        buttonStatus = false;
        txt.text = "Init";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress()
    {
        Debug.Log("pressed");
        buttonStatus = !buttonStatus;
        if (buttonStatus)
        {
            txt.text = "Pressed";
        }
        else
        {
            txt.text = "Not pressed";
        }
    }
}
