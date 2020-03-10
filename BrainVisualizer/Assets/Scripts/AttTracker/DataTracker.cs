using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DataTracker : MonoBehaviour
{
    public UnityEngine.UI.Text text;

    private MouseSensor ms;
    private double oldx;
    private double oldy;

    private KbdSensor ks;

    private void Start()
    {
        ms = new MouseSensor();
        ks = new KbdSensor();
    }

    // Update is called once per frame
    void Update()
    {
        ms.updateMousePosition();
        var x = ms.x;
        var y = ms.y;

        text.text = "dx " + (x - oldx).ToString() + "\ndy- " + (y - oldy).ToString();
        oldx = x;
        oldy = y;


        text.text += "\nkbd press - " + ks.getNumOfKeyPress().ToString();
        // ks.resetKeyPresses();

        text.text += "\nName = " + WindowData.GetActiveFileNameTitle();

        var mouseAttention = ms.getAttention();
        var mouseLikelihood = ms.getLikelihood();
        var kbdAttention = ks.getAttention();
        var kbdLikelihood = ks.getLikelihood();
        text.text += "\nAttention = " +
            ((mouseAttention * mouseLikelihood + kbdAttention * kbdLikelihood) / (mouseLikelihood + kbdLikelihood)).ToString();
    }

    public void OnDestroy()
    {
        ks.Dispose();
    }

  
}

