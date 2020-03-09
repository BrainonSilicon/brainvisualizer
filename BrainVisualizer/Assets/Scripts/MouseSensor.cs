using UnityEngine;

public class MouseSensor : IAttentionSensor
{
    public float x { get; set; }
    public float y { get; set; }

    float scrollDelta;

    public MouseSensor()
    {
        x = 0;
        y = 0;
        scrollDelta = 0;
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

    public void updateMousePosition()
    {
        x = Input.mousePosition.x;
        y = Input.mousePosition.y;
    }

    private void updateMouseScrollDelta()
    {
        scrollDelta = Input.mouseScrollDelta.y;
    }

    //public double getX()
    //{
    //    return x;
    //}

    //public double getY()
    //{
    //    return y;
    //}
}
