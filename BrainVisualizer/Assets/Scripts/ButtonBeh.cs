using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBeh : MonoBehaviour
{
    public UnityEngine.GameObject cone; 
    public UnityEngine.Sprite onImage;
    public UnityEngine.Sprite offImage;
    public UnityEngine.UI.Button button;
    bool buttonStatus;

    // Start is called before the first frame update
    void Start()
    {
        buttonStatus = false;
        button.image.sprite = offImage;
        cone.SetActive (false);

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
            button.image.sprite = onImage;
            cone.SetActive (true);
        }
        else
        {
            cone.SetActive (false);
            button.image.sprite = offImage;
        }
    }
}
