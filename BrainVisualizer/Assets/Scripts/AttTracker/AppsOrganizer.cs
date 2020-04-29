using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsOrganizer
{
    private HashSet<HashSet<string>> allGroups = new HashSet<HashSet<string>>();

    public AppsOrganizer()
    {
        InitSets();
    }

    public float Distance(string one, string two)
    {
        bool foundOne = false;
        bool foundTwo = false;
        bool foundBothInSameSet = false;
        foreach (HashSet<string> set in allGroups)
        {
            if (set.Contains(one)) foundOne = true;
            if (set.Contains(two)) foundTwo = true;
            if (set.Contains(one) && set.Contains(two)) foundBothInSameSet = true;
        }

        if (!foundOne || !foundTwo)
        {
         //   Debug.Log(one + " " + two + "20");
            return 20;
        }

        if (foundBothInSameSet)
        {
         //   Debug.Log(one + " " + two + "1");

            return 3;
        }

       // Debug.Log(one + " " + two + "10");

        return 10;
    }

    private void InitSets()
    {
        HashSet<string> set1 = new HashSet<string>();
        set1.Add("Unity");
        set1.Add("devenv");
        allGroups.Add(set1);

        HashSet<string> set2 = new HashSet<string>();
        set2.Add("mspaint");
        set2.Add("wordpad");
        allGroups.Add(set2);

        HashSet<string> set3 = new HashSet<string>();
        set3.Add("EXCEL.EXE");
        set3.Add("CHROME");
        allGroups.Add(set3);

        HashSet<string> set4 = new HashSet<string>();
        set4.Add("ApplicationFrameHost");
        allGroups.Add(set3);
    }

}
