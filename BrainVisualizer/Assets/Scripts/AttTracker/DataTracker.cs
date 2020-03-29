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
    public int clearKeypressTime;
    private int threadCounter;
    private Thread thr;
    private bool threadShouldRun;

    private MouseSensor ms;
    private double area;
    private double path;

    private KbdSensor ks;

    //public void ExampleThreadFunction()
    //{
    //    while (threadShouldRun)
    //    {
    //        threadCounter++;
    //        Thread.Sleep(threadSleepTimeInMS);
    //    }
    //}

    private void Start()
    {
        Debug.Log("Started");
        ms = new MouseSensor();
        ks = new KbdSensor();

        area = 0;

        //threadCounter = 0;
        //threadShouldRun = true;
        //thr = new Thread(ExampleThreadFunction);
        //thr.Start();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayDataOnScreen();
        UpdateData();
    }

    private void UpdateData()
    {
        var prevMouseMoving = ms.isMoving;
        ms.UpdateMouse(Input.mousePosition.x, Input.mousePosition.y);

        if (prevMouseMoving && !ms.isMoving)
        {
            area = ms.AreaNorm();
            path = ms.PathNorm();
            ms.ClearDataPoints();
        }

        ks.ClearKeyPresses(clearKeypressTime);
    }

    private void DisplayDataOnScreen()
    {
        text.text = "x- " + ms.x.ToString() + "\ny- " + ms.y.ToString();
        text.text += "\nmoving- " + ms.isMoving.ToString();
        text.text += "\narea- " + area.ToString("N3");
        text.text += "\npath- " + path.ToString("N3");
        text.text += "\nvelocity- " + (10 * ms.Velocity()).ToString("N3") + "\naccel- " + (10 * ms.Acceleration()).ToString("N3");
        text.text += "\nkbd press - " + ks.NumOfKeyPress().ToString();
        text.text += "\nName = " + WindowData.GetActiveFileNameTitle();
        text.text += "\nAttention = " +
         ((ms.Attention() * ms.Likelihood() + ks.Attention() * ks.Likelihood()) / (ms.Likelihood() + ks.Likelihood())).ToString();

        textForThread.text = threadCounter.ToString();
    }

    public void OnDestroy()
    {
        ks.Dispose();
        //   threadShouldRun = false;
    }


}

