using System;
using System.Collections.Generic;

public class KbdSensor : IAttentionSensor, IDisposable
{
    private struct KeyData
    {
        public System.DateTime t;
    }

    private List<KeyData> KeyDataPoints = new List<KeyData>();

#if UNITY_STANDALONE_WIN
    private GlobalKeyboardHook _globalKeyboardHook;
#endif
    public KbdSensor()
    {
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
            KeyData data = new KeyData();
            data.t = System.DateTime.Now;
            KeyDataPoints.Add(data);
        }
    }
#endif

    public int NumOfKeyPress()
    {
        return KeyDataPoints.Count;
    }

    public void ClearAllKeyPresses()
    {
        KeyDataPoints.Clear();
    }
    
    public void ClearKeyPresses(int t)
    {
        var currentTime = System.DateTime.Now;
        foreach (KeyData key in KeyDataPoints)
        {
            if ((currentTime - key.t).TotalSeconds > t)
            {
                KeyDataPoints.Remove(key);
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
