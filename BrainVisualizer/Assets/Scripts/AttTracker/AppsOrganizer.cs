using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsOrganizer
{
    public HashSet<HashSet<string>> allGroups;

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
            return 0;

        if (foundBothInSameSet)
            return 1;

        return 10;
    }

    private void InitSets()
    {
        HashSet<string> set1 = new HashSet<string>();
        set1.Add("Unity");
        allGroups.Add(set1);
    }

}
