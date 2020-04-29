using System;
using System.Threading;
using UnityEngine;
using System.Diagnostics;



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
    public ChartDrawer tasksChart;
    public ChartDrawer smallMouseAttentionIndicator;
    public ChartDrawer smallTaskAttentionIndicator;
    public ChartDrawer smallKbdAttentionIndicator;
    public GameObject ARObject;
    public GameObject Head;
    private int threadCounter;
    private Thread thr;
    private bool threadShouldRun;
    private bool appSwitch = false;
    public bool randomData = true;
    private float tasksAttention = 0.0f;
    private float mouseAttention = 0.0f;
    private float kbdAttention = 0.0f;
    private System.DateTime lastKbdCheckTime;


    //  private ChromeCatchData chromeCatcher;

    public MouseSensor ms;
    private double area;
    private double path;

    private AppsOrganizer appsOrganizer = new AppsOrganizer();

    public double Attention;

    private String activeWindow;

    public KbdSensor ks;

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
        // ks = new KbdSensor();

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

        lastKbdCheckTime = DateTime.Now;

        //Debug.Log("About to start Monitoring Chrome");
        //chromeCatcher = new ChromeCatchData();
        //chromeCatcher.StartMonitoring();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayDataOnScreen();
        UpdateData();
        ks.isRandom = randomData;

        //Debug.Log("Chrome is active - " + chromeCatcher.IsActive.ToString());
        //Debug.Log("Chrome current tab - " + chromeCatcher.CurrentTab);

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
        ks.ClearKeyPresses(clearKeypressTime * 2);   // we want to keep twice as many data points so we can calculate the second derivative
        ks.ClearWords(clearKeypressTime * 2);   // we want to keep twice as many data points so we can calculate the second derivative
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
        var keyPress = ks.NumOfKeyPress(clearKeypressTime);
        keyboardText.text += "Key - " + ((float)keyPress / clearKeypressTime).ToString("N2") + "Hz";
        keyboardText.text += ", Word - " + ((float)ks.NumOfWords(clearKeypressTime) / clearKeypressTime).ToString("N2") + "Hz";
        keyboardText.text += "\n#Key - " + keyPress;
        keyboardText.text += ", #Word - " + ks.NumOfWords(clearKeypressTime);
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
        if (activeWindow == "ApplicationFrameHost") activeWindow = "Explorer - Facebook";
        if (activeWindow == "Chrome") activeWindow = "Chrome - GMail";
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
        //    chromeCatcher.StopMonitoring();
        //   threadShouldRun = false;
    }

    private void AttentionUpdate(bool mouseStop)
    {
        if (mouseStop) MousePathAttentionUpdate();
        AppSwitchAttentionUpdate();
        KbdAttentionUpdate();
    }

    public void KbdAttentionUpdate()
    {
        if ((DateTime.Now - lastKbdCheckTime).TotalSeconds > 3)
        {
            lastKbdCheckTime = DateTime.Now;
            var dt = clearKeypressTime;
            var keyCountt0 = ks.NumOfKeyPress(clearKeypressTime);
            var keyCountt1 = ks.NumOfKeyPress(clearKeypressTime * 2) - keyCountt0;

            var dkeyCount0dt = keyCountt0 / dt;
            var dkeyCount1dt = keyCountt1 / dt;

            var keyCountChange = (dkeyCount0dt - dkeyCount1dt);

            if (keyCountChange > 0.3) kbdAttention += ChangeAttentionPoints(3);
            else if (keyCountChange > -0.3) kbdAttention += ChangeAttentionPoints(1);
            else kbdAttention += ChangeAttentionPoints(-3);

            smallKbdAttentionIndicator.AddDataPoint(kbdAttention);
            smallKbdAttentionIndicator.DrawLine();
        }

    }

    private void MousePathAttentionUpdate()
    {
        float attentionAdded;
        if (path < MousePathRatioThreshold)
        {
            attentionAdded = ChangeAttentionPoints(3);
            ms.DrawPath(true); //, MousePathStartWin, MousePathEndWin);
        }
        else
        {
            attentionAdded = ChangeAttentionPoints(-3);
            ms.DrawPath(false); //, MousePathStartWin, MousePathEndWin);
        }

        ms.DrawStartEnd(); //, MousePathStartWin, MousePathEndWin);
        mouseAttention += attentionAdded;
        smallMouseAttentionIndicator.AddDataPoint(mouseAttention);
        smallMouseAttentionIndicator.DrawLine();
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
                tasksChart.AddDataPoint(appsOrganizer.Distance(activeWindow, WindowData.GetActiveFileNameTitle()));
                var attentionAdded = ChangeAttentionPoints(-appsOrganizer.Distance(activeWindow, WindowData.GetActiveFileNameTitle()));
                tasksAttention += attentionAdded;
                smallTaskAttentionIndicator.AddDataPoint(tasksAttention);
                smallTaskAttentionIndicator.DrawLine();
                activeWindow = WindowData.GetActiveFileNameTitle();

            }
        }
    }


    private void DisableText()
    {
        textAppSwitch.enabled = false;
    }

    private float ChangeAttentionPoints(double points)
    {
        double attToAdd;
        if (points > 0)
        {
            var dAttention = 100 - Attention;
            attToAdd = points * dAttention / 100;
        }
        else
        {
            attToAdd = points * Attention / 100;

        }
        Attention += attToAdd;

        if (Attention > 100) Attention = 100;
        if (Attention < 0) Attention = 0;
        return (float)attToAdd;
    }

    //private void GetChrome()
    //{// there are always multiple chrome processes, so we have to loop through all of them to find the
    // // process with a Window Handle and an automation element of name "Address and search bar"
    //    Process[] procsChrome = Process.GetProcessesByName("chrome");
    //    foreach (Process chrome in procsChrome)
    //    {
    //        // the chrome process must have a window
    //        if (chrome.MainWindowHandle == IntPtr.Zero)
    //        {
    //            continue;
    //        }

    //        // find the automation element
    //        AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);

    //        // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
    //        AutomationElement elmUrlBar = null;
    //        try
    //        {
    //            // walking path found using inspect.exe (Windows SDK) for Chrome 31.0.1650.63 m (currently the latest stable)
    //            var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
    //            if (elm1 == null) { continue; } // not the right chrome.exe
    //                                            // here, you can optionally check if Incognito is enabled:
    //                                            //bool bIncognito = TreeWalker.RawViewWalker.GetFirstChild(TreeWalker.RawViewWalker.GetFirstChild(elm1)) != null;
    //            var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1); // I don't know a Condition for this for finding :(
    //            var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
    //            var elm4 = elm3.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
    //            elmUrlBar = elm4.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom));
    //        }
    //        catch
    //        {
    //            // Chrome has probably changed something, and above walking needs to be modified. :(
    //            // put an assertion here or something to make sure you don't miss it
    //            continue;
    //        }

    //        // make sure it's valid
    //        if (elmUrlBar == null)
    //        {
    //            // it's not..
    //            continue;
    //        }

    //        // elmUrlBar is now the URL bar element. we have to make sure that it's out of keyboard focus if we want to get a valid URL
    //        if ((bool)elmUrlBar.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
    //        {
    //            continue;
    //        }

    //        // there might not be a valid pattern to use, so we have to make sure we have one
    //        AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
    //        if (patterns.Length == 1)
    //        {
    //            string ret = "";
    //            try
    //            {
    //                ret = ((ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0])).Current.Value;
    //            }
    //            catch { }
    //            if (ret != "")
    //            {
    //                // must match a domain name (and possibly "https://" in front)
    //                if (Regex.IsMatch(ret, @"^(https:\/\/)?[a-zA-Z0-9\-\.]+(\.[a-zA-Z]{2,4}).*$"))
    //                {
    //                    // prepend http:// to the url, because Chrome hides it if it's not SSL
    //                    if (!ret.StartsWith("http"))
    //                    {
    //                        ret = "http://" + ret;
    //                    }
    //                    Console.WriteLine("Open Chrome URL found: '" + ret + "'");
    //                }
    //            }
    //            continue;
    //        }
    //    }
    //}
}

