using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Runtime.InteropServices;


public class DataTracker : MonoBehaviour
{
    public UnityEngine.UI.Text mousetext;
    public UnityEngine.UI.Text keyboardtext;
    public UnityEngine.UI.Text appNameText;
    public UnityEngine.UI.Text textAttention;
    public UnityEngine.UI.Text textAppSwitch;
    public UnityEngine.UI.Text textAllAppTime;
    public int threadSleepTimeInMS;
    public int clearKeypressTime;
    public double MousePathRatioThreshold;
    private int threadCounter;
    private Thread thr;
    private bool threadShouldRun;
    private bool appSwitch = false;

    public MouseSensor ms;
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
        //   ms = new MouseSensor();
        ks = new KbdSensor();

        area = 0;
        Application.targetFrameRate = 10;
        //threadCounter = 0;
        //threadShouldRun = true;
        //thr = new Thread(ExampleThreadFunction);
        //thr.Start();

        textAppSwitch.enabled = false;
        textAllAppTime.text = WindowData.GetActiveFileNameTitle() + ": ";
        textAllAppTime.text += DateTime.Now.ToString("h:mm:ss tt");
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

        Vector3 screenPos = Input.mousePosition;
        //  screenPos.z = 10.0f;
        // Vector3 canvasPos = Camera.main.ScreenToWorldPoint(screenPos);

        ms.UpdateMouse(screenPos.x, screenPos.y);
        bool mouseStop = prevMouseMoving && !ms.isMoving;

        activeWindow = WindowData.GetActiveFileNameTitle();

        if (mouseStop)
        {
            area = ms.AreaNorm();
            path = ms.PathNorm();
        }
        AttentionUpdate(mouseStop);

        if (mouseStop) ms.ClearDataPoints();
        ks.ClearKeyPresses(clearKeypressTime);
    }

    private void DisplayDataOnScreen()
    {
        DisplayMouseText();
        DisplayKBDText();
        DisplayAppText();
        DisplayAttentionText();

        //  text.text += "\nAttention = " +
        //  ((ms.Attention() * ms.Likelihood() + ks.Attention() * ks.Likelihood()) / (ms.Likelihood() + ks.Likelihood())).ToString();


    }

    public void DisplayMouseText()
    {
        mousetext.text = "";
        mousetext.text += "x- " + ms.x.ToString() + "\ny- " + ms.y.ToString();
        mousetext.text += "\nmoving- " + ms.isMoving.ToString();
        mousetext.text += "\narea- " + area.ToString("N6");
        mousetext.text += "\npath- " + path.ToString("N6");
        mousetext.text += "\nvelocity- " + (10 * ms.Velocity()).ToString("N3") + "\naccel- " + (10 * ms.Acceleration()).ToString("N3");
    }

    public void DisplayKBDText()
    {
        keyboardtext.text = "";
        keyboardtext.text += "kbd press - " + ks.NumOfKeyPress().ToString();
    }

    public void DisplayAppText()
    {
        appNameText.text = "Name = " + activeWindow;
        AppSwitchAttentionDisplayUpdate();
    }

    public void DisplayAttentionText()
    {
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
        if (path < MousePathRatioThreshold)
        {
            ChangeAttentionPoints(3);
            ms.DrawPath(true); //, MousePathStartWin, MousePathEndWin);
        }
        else
        {
            ChangeAttentionPoints(-3);
            ms.DrawPath(false); //, MousePathStartWin, MousePathEndWin);
        }



        ms.DrawStartEnd(); //, MousePathStartWin, MousePathEndWin);
    }

    private void AppSwitchAttentionUpdate()
    {
        if (appSwitch)
        {
            ChangeAttentionPoints(-10);
            appSwitch = false;
        }
    }

    private void AppSwitchAttentionDisplayUpdate()
    {
        if (activeWindow != WindowData.GetActiveFileNameTitle())
        {
            textAppSwitch.enabled = true;
            Invoke("DisableText", 1f);

            if (WindowData.GetActiveFileNameTitle() != "Unknown")
            {
                textAllAppTime.text += " - " + DateTime.Now.ToString("h:mm:ss tt") + "\n";
                textAllAppTime.text += WindowData.GetActiveFileNameTitle() + ": ";
                textAllAppTime.text += DateTime.Now.ToString("h:mm:ss tt");
            }
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

