using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Runtime.InteropServices;


public class DataTracker : MonoBehaviour
{
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Text textForThread;
    public int threadSleepTimeInMS;
    private int threadCounter;
    private Thread thr;
    private bool threadShouldRun;

    private MouseSensor ms;
    private double oldx;
    private double oldy;

    private KbdSensor ks;

    public void ExampleThreadFunction()
    {
        while (threadShouldRun)
        {
            threadCounter++;
            Thread.Sleep(threadSleepTimeInMS); 
        }
    }

    private void Start()
    {
        ms = new MouseSensor();
        ks = new KbdSensor();

        threadCounter = 0;
        threadShouldRun = true;
        thr = new Thread(ExampleThreadFunction);
        thr.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //everytime you move the mouse, this updates to the 'relative' new position from old 
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


        textForThread.text = threadCounter.ToString();
    }

    public void OnDestroy()
    {
        ks.Dispose();
        threadShouldRun = false;
    }

  
}

