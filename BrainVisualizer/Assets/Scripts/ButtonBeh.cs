using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBeh : MonoBehaviour
{
    public UnityEngine.UI.Text txt;
    public UnityEngine.Sprite onImage;
    public UnityEngine.Sprite offImage;
    public UnityEngine.UI.Button button;
    bool buttonStatus;

    // Start is called before the first frame update
    void Start()
    {
        buttonStatus = false;
        button.image.sprite = offImage;
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
            button.image.sprite = onImage;
        }
        else
        {
            txt.text = "Not pressed";
            button.image.sprite = offImage;
        }
    }
}
