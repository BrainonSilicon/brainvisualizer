using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;

class ChromeCatchData
{
    const int NumTopTabs = 10;

    List<int> Counts = new List<int>();
    public ArrayList AllTabsUsed = new ArrayList();
    int IndexProcesChrome = -1;
    Process[] List;

    public int Delay = 250;
    public bool IsActive { get; private set; }

    public string CurrentTab = "";
    public string[] TopUsedTabs = new string[NumTopTabs];


    public bool IsChromeOpened()
    {
        //UnityEngine.Debug.Log("Checking if chrome is open");
        //List = Process.GetProcesses();
        //foreach (Process p in List)
        //{
        //    UnityEngine.Debug.Log(p.ProcessName);
        //}

        List = Process.GetProcessesByName("chrome");
        if (List.Count() == 0)
            return false;
        else
            return true;
    }


    public async void StartMonitoring()
    {
        IsActive = true;
        while (IsActive)
        {
            await Task.Delay(Delay);
            if (!IsChromeOpened())
            {
                StopMonitoring();
                break;
            }
            else
            { 
                IndexProcesChrome = -1;
                UnityEngine.Debug.Log("List size " + List.Count().ToString());
                foreach (Process p in List)
                {
                    UnityEngine.Debug.Log("Process name " + p.ProcessName + " "
                        + p.MainWindowTitle);// + " " + p.
                    IndexProcesChrome += 1;
                    //if (p.MainWindowTitle != "")
                    //    break;
                }
                Process Proc = List[IndexProcesChrome];
                UnityEngine.Debug.Log("Decision - " + Proc.MainWindowTitle);
                try
                {
                    CurrentTab = Proc.MainWindowTitle.Remove(Proc.MainWindowTitle.Length - 15);
                    if (!AllTabsUsed.Contains(Proc.MainWindowTitle))
                    {
                        AllTabsUsed.Add(Proc.MainWindowTitle);
                        Counts.Add(1);
                    }
                    else
                        Counts[AllTabsUsed.IndexOf(Proc.MainWindowTitle)] += 1;
                    OrderMostUsedTabs();
                }
                catch (Exception e) { };
            }
        }
    }

    private void OrderMostUsedTabs()
    {
        int[] TempCounts = Counts.ToArray();
        for (int i = 0; (i < NumTopTabs) && (i < Counts.Count); i++)
        {
            int Max = 0;
            for (int j = 0; j < Counts.Count; j++)
                if ((TempCounts[j] > TempCounts[Max]))
                    Max = j;
            string str = AllTabsUsed[Max].ToString();
            TopUsedTabs[i] = str.Remove(str.Length - 15);
            TempCounts[Max] = -1;
        }
    }

    public void StopMonitoring()
    {
        IsActive = false;
        CurrentTab = "";
    }
}
