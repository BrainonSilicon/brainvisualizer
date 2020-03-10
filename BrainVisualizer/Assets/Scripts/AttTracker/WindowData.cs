using System;
using System.Runtime.InteropServices;

public class WindowData
{
#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
#endif

    public static String GetActiveFileNameTitle()
    {
#if UNITY_STANDALONE_WIN
        IntPtr hWnd = GetForegroundWindow();
        uint processId;
        GetWindowThreadProcessId(hWnd, out processId);
        try
        { 
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById((int)processId);
            return p.ProcessName;
        }
        catch 
        {
            return "Unknown";
        }
#else
        return "Not implemented";
#endif
    }
}
