using System;
using System.Runtime.InteropServices;

public class WindowData
{
#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
#else
 //   [DllImport(@"/System/Library/Frameworks/QuartzCore.framework/QuartzCore")]
 //   static extern IntPtr CGWindowListCopyWindowInfo(MonoMac.CoreGraphics.CGWindowListOption option, uint relativeToWindow);
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
        // [Foundation.Register("NSWorkspace", true)]
        //public class NSWorkspace : Foundation.NSObject


        ////   string result = null;
        //IntPtr windowInfo = CGWindowListCopyWindowInfo(MonoMac.CoreGraphics.CGWindowListOption.OnScreenOnly, 0);
        
        //MonoMac.Foundation.NSArray values = (MonoMac.Foundation.NSArray)MonoMac.ObjCRuntime.Runtime.GetNSObject(windowInfo);

        
       

        //for (ulong i = 0, len = values.Count; i < len; i++)
        //{
        //    MonoMac.Foundation.NSObject window = MonoMac.ObjCRuntime.Runtime.GetNSObject(values.ValueAt(i));

        //    MonoMac.Foundation.NSString key = new MonoMac.Foundation.NSString("kCGWindowOwnerPID");
        //    MonoMac.Foundation.NSNumber value = (MonoMac.Foundation.NSNumber)window.ValueForKey(key);
        //    // and so on
        //}
        
        
     //   var foreground_app = MonoMac.AppKit.NSWorkspace.SharedWorkspace.FrontmostApplication;
     //   return foreground_app.LocalizedName;
   //    Console.WriteLine($"Pid: {foreground_app.ProcessIdentifier}");
        //var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
        //return currentProcess.ProcessName;
            return "Not implemented";
#endif
    }
}
