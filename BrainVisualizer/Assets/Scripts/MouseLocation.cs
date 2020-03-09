using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MouseLocation : MonoBehaviour
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

        text.text += "\nName = " + GetActiveFileNameTitle();


    }

    public void OnDestroy()
    {
        ks.Dispose();
    }

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    public String GetActiveFileNameTitle()
    {
        IntPtr hWnd = GetForegroundWindow();
        uint processId;
        GetWindowThreadProcessId(hWnd, out processId);
        System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById((int)processId);
        //p.MainModule.FileName.Dump();
        return p.ProcessName;
    }

}

