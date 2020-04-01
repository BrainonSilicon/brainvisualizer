using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Runtime.InteropServices;


public class DataTracker : MonoBehaviour
{
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Text textAttention;
    public UnityEngine.UI.Text textAppSwitch;
    public int threadSleepTimeInMS;
    public int clearKeypressTime;
    public GameObject MousePath;
    public GameObject MouseStartEnd;
    public Color MousePathColorHighAttention;
    public Color MousePathColorLowAttention;
    public Color MouseStartEndColor;
    public Vector3 MousePathStartWin;
    public Vector3 MousePathEndWin;
    private int threadCounter;
    private Thread thr;
    private bool threadShouldRun;

    private MouseSensor ms;
    private double area;
    private double path;

    private double Attention;

    private String activeWindow;

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
        ms = new MouseSensor();
        ks = new KbdSensor();

        area = 0;
        Application.targetFrameRate = 10;
        //threadCounter = 0;
        //threadShouldRun = true;
        //thr = new Thread(ExampleThreadFunction);
        //thr.Start();

     
        textAppSwitch.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayDataOnScreen();
        UpdateData();
    }

    private void UpdateData()
    {
        activeWindow = WindowData.GetActiveFileNameTitle();
        var prevMouseMoving = ms.isMoving;
        ms.UpdateMouse(Input.mousePosition.x, Input.mousePosition.y);

        if (prevMouseMoving && !ms.isMoving)
        {
            area = ms.AreaNorm();
            path = ms.PathNorm();
            AttentionUpdate();
            ms.ClearDataPoints();
        }

        ks.ClearKeyPresses(clearKeypressTime);
    }

    private void DisplayDataOnScreen()
    {
        text.text = "x- " + ms.x.ToString() + "\ny- " + ms.y.ToString();
        text.text += "\nmoving- " + ms.isMoving.ToString();
        text.text += "\narea- " + area.ToString("N6");
        text.text += "\npath- " + path.ToString("N6");
        var v = ms.Velocity();
        text.text += "\nvelocity- " + (10 * ms.Velocity()).ToString("N3") + "\naccel- " + (10 * ms.Acceleration()).ToString("N3");
        text.text += "\nkbd press - " + ks.NumOfKeyPress().ToString();
        text.text += "\nName = " + activeWindow;
        //  text.text += "\nAttention = " +
        //  ((ms.Attention() * ms.Likelihood() + ks.Attention() * ks.Likelihood()) / (ms.Likelihood() + ks.Likelihood())).ToString();

        textAttention.text = "Attention - " + Attention.ToString();
    }

    public void OnDestroy()
    {
        ks.Dispose();
        //   threadShouldRun = false;
    }

    private void AttentionUpdate()
    {
        MousePathAttentionUpdate();
        AppSwitchAttentionUpdate();
    }

    private void MousePathAttentionUpdate()
    {
        var dAttention = 100 - Attention;
        if (path < 1.5)
        {
            Attention += 3.0 * dAttention / 100;
            ms.DrawPath(MousePath, MousePathColorHighAttention, MousePathStartWin, MousePathEndWin);
        }
        else
        {
            Attention -= 3.0 * dAttention / 100;
            ms.DrawPath(MousePath, MousePathColorLowAttention, MousePathStartWin, MousePathEndWin);
        }

        if (Attention > 100) Attention = 100;
        if (Attention < 0) Attention = 0;

        ms.DrawStartEnd(MouseStartEnd, MouseStartEndColor, MousePathStartWin, MousePathEndWin);
    }

    private void AppSwitchAttentionUpdate()
    {
        if (activeWindow != WindowData.GetActiveFileNameTitle())
        {
            textAppSwitch.enabled = true;
            Invoke("DisableText", 1f);

        }
    }

    void DisableText()
    {
        textAppSwitch.enabled = false;
    }
}

