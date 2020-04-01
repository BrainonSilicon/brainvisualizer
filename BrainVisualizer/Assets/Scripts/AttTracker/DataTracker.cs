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

    public double Attention;

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
        var prevMouseMoving = ms.isMoving;
        ms.UpdateMouse(Input.mousePosition.x, Input.mousePosition.y);
        bool mouseStop = prevMouseMoving && !ms.isMoving;
        AttentionUpdate(mouseStop);

        activeWindow = WindowData.GetActiveFileNameTitle();

        if (mouseStop)
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
        text.text += "\narea- " + area.ToString("N6");
        text.text += "\npath- " + path.ToString("N6");
        var v = ms.Velocity();
        text.text += "\nvelocity- " + (10 * ms.Velocity()).ToString("N3") + "\naccel- " + (10 * ms.Acceleration()).ToString("N3");
        text.text += "\nkbd press - " + ks.NumOfKeyPress().ToString();
        text.text += "\nName = " + activeWindow;
        //  text.text += "\nAttention = " +
        //  ((ms.Attention() * ms.Likelihood() + ks.Attention() * ks.Likelihood()) / (ms.Likelihood() + ks.Likelihood())).ToString();

        textAttention.text = "Attention - " + Attention.ToString("N1");
    }

    public void OnDestroy()
    {
        ks.Dispose();
        //   threadShouldRun = false;
    }

    private void AttentionUpdate(bool mouseStop)
    {
        if (mouseStop) MousePathAttentionUpdate();
        AppSwitchAttentionUpdate();
    }

    private void MousePathAttentionUpdate()
    {
        if (path < 1.5)
        {
            ChangeAttentionPoints(3);
            ms.DrawPath(MousePath, MousePathColorHighAttention, MousePathStartWin, MousePathEndWin);
        }
        else
        {
            ChangeAttentionPoints(-3);
            ms.DrawPath(MousePath, MousePathColorLowAttention, MousePathStartWin, MousePathEndWin);
        }

      

        ms.DrawStartEnd(MouseStartEnd, MouseStartEndColor, MousePathStartWin, MousePathEndWin);
    }

    private void AppSwitchAttentionUpdate()
    {
        if (activeWindow != WindowData.GetActiveFileNameTitle())
        {
            ChangeAttentionPoints(-10);
            textAppSwitch.enabled = true;
            Invoke("DisableText", 1f);

        }
    }

    private void DisableText()
    {
        textAppSwitch.enabled = false;
    }

    private void ChangeAttentionPoints(double points)
    {
        if (points > 0)
        {
            var dAttention = 100 - Attention;
            Attention += points * dAttention / 100;
        }
        else
        {
            Attention += points * Attention / 100;
        }

        if (Attention > 100) Attention = 100;
        if (Attention < 0) Attention = 0;
    }
}

