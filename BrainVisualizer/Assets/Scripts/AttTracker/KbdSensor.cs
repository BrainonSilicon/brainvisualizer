using System;

public class KbdSensor : IAttentionSensor, IDisposable
{
    private int _keyPressed;
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
            _keyPressed++;
        }
    }
#endif

    public int getNumOfKeyPress()
    {
        return _keyPressed;
    }

    public void resetKeyPresses()
    {
        _keyPressed = 0;
    }

    public double getAttention()
    {
        return 0.5;
    }

    public double getLikelihood()
    {
        return 0.5;
    }

    public void getAttentionAndLikelihood(ref double attention, ref double likelihood)

    //  public void getAttentionAndLikelihood(ref double attention, ref double likelihood)
    {
        attention = getAttention();
        likelihood = getLikelihood();
    }

    public bool isActive()
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
