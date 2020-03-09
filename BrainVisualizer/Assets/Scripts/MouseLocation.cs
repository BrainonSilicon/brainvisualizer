using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocation : MonoBehaviour
{
    public UnityEngine.UI.Text text;

    private double oldx;
    private double oldy;

    // Update is called once per frame
    void FixedUpdate()
    {
        var pos = Input.mousePosition;

        text.text = "dx " + (pos.x - oldx).ToString() + "\ndy- " + (pos.y - oldy).ToString() ;
        oldx = pos.x;
        oldy = pos.y;


        //if (Event.current.type == EventType.KeyDown)
        //{
        //    text.text += "\n press";
        //}

        var kbdClick = Input.anyKey;
        if (kbdClick)
        {
            text.text += "\nkey down";
        }
        else
        {
            text.text += "\nno click";           
        }

    }
}
