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

    public int NumOfKeyPress()
    {
        return _keyPressed;
    }

    public void ResetKeyPresses()
    {
        _keyPressed = 0;
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
