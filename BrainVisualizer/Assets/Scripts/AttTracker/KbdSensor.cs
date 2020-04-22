using System;
using System.Collections.Generic;
using UnityEngine;

public class KbdSensor : MonoBehaviour, IAttentionSensor, IDisposable
{
    public bool isRandom { get; set; }

    private struct KeyData
    {
        public System.DateTime t;
    }

    private List<KeyData> KeyDataPoints = new List<KeyData>();
    private List<KeyData> WordsDataPoints = new List<KeyData>();

#if UNITY_STANDALONE_WIN
    private GlobalKeyboardHook _globalKeyboardHook;
#endif
    public KbdSensor()
    {
//#if UNITY_STANDALONE_WIN
//        _globalKeyboardHook = new GlobalKeyboardHook();
//        _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
//#endif
    }

    private void Start()
    {
        Debug.Log("Started");
#if UNITY_STANDALONE_WIN
        _globalKeyboardHook = new GlobalKeyboardHook();
        _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
#endif
    }

#if UNITY_STANDALONE_WIN
    private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
    {
        if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
        {
            Debug.Log("Key down");
            KeyData data = new KeyData();
            data.t = System.DateTime.Now;
            KeyDataPoints.Add(data);
            if ((e.KeyboardData.VirtualCode == 32) ||
                (e.KeyboardData.VirtualCode == 13))
            {
                WordsDataPoints.Add(data);
            }
        }
    }
#endif

    public int NumOfKeyPress()
    {
        if (isRandom)
        {
            return UnityEngine.Random.Range(1, 30);
        }

        return KeyDataPoints.Count;
    }



    public int NumOfKeyPress(int t)
    {
        int res = 0;
        var currentTime = System.DateTime.Now;
        foreach (KeyData key in KeyDataPoints)
        {
            if ((currentTime - key.t).TotalSeconds < t)
            {
                res++;
            }
        }

        return res;
    }

    public int NumOfWords()
    {
        return WordsDataPoints.Count;
    }

    public int NumOfWords(int t)
    {
        int res = 0;
        var currentTime = System.DateTime.Now;
        foreach (KeyData word in WordsDataPoints)
        {
            if ((currentTime - word.t).TotalSeconds < t)
            {
                res++;
            }
        }

        return res;
    }

    public void ClearAllKeyPresses()
    {
        KeyDataPoints.Clear();
    }

    public void ClearAllWords()
    {
        WordsDataPoints.Clear();
    }

    public void ClearKeyPresses(int t)
    {
        var currentTime = System.DateTime.Now;
        foreach (KeyData key in KeyDataPoints)
        {
            try
            {
                if ((currentTime - key.t).TotalSeconds > t)
                {
                    KeyDataPoints.Remove(key);
                }
            } catch
            {
                // do nothing
            }
        }
    }

    public void ClearWords(int t)
    {
        var currentTime = System.DateTime.Now;
        foreach (KeyData word in WordsDataPoints)
        {
            try
            {
                if ((currentTime - word.t).TotalSeconds > t)
                {
                    WordsDataPoints.Remove(word);
                }
            }
            catch
            {
                // do nothing
            }
        }
    }

    public double Attention()
    {
        return 0.5;
    }

    public double Likelihood()
    {
        return 0.5;
    }

    public void AttentionAndLikelihood(ref double attention, ref double likelihood)

    //  public void getAttentionAndLikelihood(ref double attention, ref double likelihood)
    {
        attention = Attention();
        likelihood = Likelihood();
    }

    public bool IsActive()
    {
        return true;
    }

    public void Dispose()
    {
#if UNITY_STANDALONE_WIN
        _globalKeyboardHook.KeyboardPressed -= OnKeyPressed;
        _globalKeyboardHook?.Dispose();
#endif
    }

}
