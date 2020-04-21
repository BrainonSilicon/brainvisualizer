using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Runtime.InteropServices;


public class DataTracker : MonoBehaviour
{
    public UnityEngine.UI.Text mouseText;
    public UnityEngine.UI.Text keyboardText;
    public UnityEngine.UI.Text appNameText;
    public UnityEngine.UI.Text headText;
    public UnityEngine.UI.Text textAttention;
    public UnityEngine.UI.Text textAppSwitch;
    public UnityEngine.UI.Text textAllAppTime;
    public int threadSleepTimeInMS;
    public int clearKeypressTime;
    public double MousePathRatioThreshold;
    public TaskChartDrawer tasksChart;
    public GameObject ARObject;
    public GameObject Head;
    private int threadCounter;
    private Thread thr;
    private bool threadShouldRun;
    private bool appSwitch = false;


    private ChromeCatchData chromeCatcher;

    public MouseSensor ms;
    private double area;
    private double path;

    private AppsOrganizer appsOrganizer = new AppsOrganizer();

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
        textAllAppTime.text = DateTime.Now.ToString("h:mm  ");
        activeWindow = WindowData.GetActiveFileNameTitle();
        textAllAppTime.text += activeWindow;

        Debug.Log("About to start Monitoring Chrome");
        chromeCatcher = new ChromeCatchData();
        chromeCatcher.StartMonitoring();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayDataOnScreen();
        UpdateData();

   //     Debug.Log("Chrome is active - "  + chromeCatcher.IsActive.ToString());
   //     Debug.Log("Chrome current tab - " + chromeCatcher.CurrentTab );

    }

    private void UpdateData()
    {
        var prevMouseMoving = ms.isMoving;

        Vector3 screenPos = Input.mousePosition;
        //  screenPos.z = 10.0f;
        // Vector3 canvasPos = Camera.main.ScreenToWorldPoint(screenPos);

        ms.UpdateMouse(screenPos.x, screenPos.y);
        bool mouseStop = prevMouseMoving && !ms.isMoving;


        if (mouseStop)
        {
            area = ms.AreaNorm();
            path = ms.PathNorm();
        }
        AttentionUpdate(mouseStop);

        if (mouseStop) ms.ClearDataPoints();
        ks.ClearKeyPresses(clearKeypressTime);
        ks.ClearWords(clearKeypressTime);
    }

    private void DisplayDataOnScreen()
    {
        DisplayMouseText();
        DisplayKBDText();
        DisplayAppText();
        DisplayHeadText();
        RotateHead();
        DisplayAttentionText();
        DisplayTasksChart();
        //  text.text += "\nAttention = " +
        //  ((ms.Attention() * ms.Likelihood() + ks.Attention() * ks.Likelihood()) / (ms.Likelihood() + ks.Likelihood())).ToString();


    }

    private void DisplayTasksChart()
    {
        tasksChart.DrawLine();
    }

    private void DisplayMouseText()
    {
        mouseText.text = "";
        //   mousetext.text += "x- " + ms.x.ToString() + "\ny- " + ms.y.ToString();
        //   mousetext.text += "\nmoving- " + ms.isMoving.ToString();
        mouseText.text += "area- " + area.ToString("N2");
        mouseText.text += ",      path- " + path.ToString("N2");
        mouseText.text += "\nvelocity- " + (10 * ms.Velocity()).ToString("N2");
        mouseText.text += ", accel- " + (10 * ms.Acceleration()).ToString("N2");
    }

    public void DisplayKBDText()
    {
        keyboardText.text = "";
        keyboardText.text += "Key - " + ((float)ks.NumOfKeyPress()* clearKeypressTime/60).ToString("N2") + "Hz";
        keyboardText.text += ", Word - " + ((float)ks.NumOfWords()*clearKeypressTime/60).ToString("N2")+ "Hz";
    }

    private void DisplayHeadText()
    {
        headText.text = "rx - " + ARObject.transform.rotation.eulerAngles.x.ToString("N1");
        headText.text += ", ry - " + ARObject.transform.rotation.eulerAngles.y.ToString("N1");
        headText.text += ", rz - " + ARObject.transform.rotation.eulerAngles.z.ToString("N1");
    }

    private void RotateHead()
    {
        Head.transform.rotation = ARObject.transform.rotation;
    }

    private void DisplayAppText()
    {
        appNameText.text = "Name = " + activeWindow;
        AppSwitchAttentionDisplayUpdate();
    }

    private void DisplayAttentionText()
    {
        textAttention.text = "Attention - " + Attention.ToString("N1");
    }

    public void OnDestroy()
    {
        ks.Dispose();
        chromeCatcher.StopMonitoring();
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
            if (WindowData.GetActiveFileNameTitle() != "Unknown")
            {
                textAppSwitch.enabled = true;
                Invoke("DisableText", 1f);

                textAllAppTime.text += "\n" + DateTime.Now.ToString("h:mm  ");
                textAllAppTime.text += WindowData.GetActiveFileNameTitle();
                //var rect = textAllAppTime.rectTransform.rect;
                //rect.height += 46;
                tasksChart.AddDataPoint(appsOrganizer.Distance(activeWindow, WindowData.GetActiveFileNameTitle()));
                activeWindow = WindowData.GetActiveFileNameTitle();

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

