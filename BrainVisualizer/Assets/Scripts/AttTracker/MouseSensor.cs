using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseSensor : IAttentionSensor
{
    struct MouseData
    {
        public double x;
        public double y;
        public System.DateTime t;
    }


    public float x { get; private set; }
    public float y { get; private set; }

    public bool isMoving { get; private set; }

 //   const int DATAPOINTS = 1000;
    private List<MouseData> mouseDataPoints = new List<MouseData>();

    float scrollDelta;

    public MouseSensor()
    {
        x = 0;
        y = 0;
        scrollDelta = 0;
        isMoving = false;
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

    public void UpdateMouse()
    {
        var newx = Input.mousePosition.x;
        var newy = Input.mousePosition.y;

        if ((newx == x) && (newy == y))
        {
            isMoving = false;
            return;
        }

        isMoving = true;

        x = newx;
        y = newy;

        MouseData dataPoint = new MouseData();
        dataPoint.x = x;
        dataPoint.y = y;
        dataPoint.t = System.DateTime.Now;
        mouseDataPoints.Add(dataPoint);
        //if (mouseDataPoints.Count > DATAPOINTS)
        //{
        //    mouseDataPoints.RemoveAt(0);
        //}
    }

    private void UpdateMouseScrollDelta()
    {
        scrollDelta = Input.mouseScrollDelta.y;
    }

    public double Velocity()
    {
        // velocity is dx/dt
        return Velocity(0);
    }


    // to get current velocity use i=0
    // to get velocity in the previous sample use i=1
    // etc.
    public double Velocity(int i)
    {
        // velocity is dx/dt
        if (mouseDataPoints.Count - i  < 2)
        {
            return 0;
        }

        var dx = mouseDataPoints[mouseDataPoints.Count - i - 1].x - mouseDataPoints[mouseDataPoints.Count - i - 2].x;
        var dy = mouseDataPoints[mouseDataPoints.Count - i - 1].y - mouseDataPoints[mouseDataPoints.Count - i - 2].y;
        var dt = mouseDataPoints[mouseDataPoints.Count - i - 1].t - mouseDataPoints[mouseDataPoints.Count - i - 2].t;

        return Mathf.Sqrt((float)((dx * dx + dy * dy))) / dt.TotalMilliseconds;
    }

    public double Acceleration()
    {
        return Acceleration(0);
    }

    public double Acceleration(int i)
    {
        // velocity is dv/dt
        if (mouseDataPoints.Count - i < 3)
        {
            return 0;
        }

        var v1 = Velocity(0);
        var v2 = Velocity(1);
        var dt = mouseDataPoints[mouseDataPoints.Count - i - 1].t - mouseDataPoints[mouseDataPoints.Count - i - 2].t;

        return (v1-v2) / dt.TotalMilliseconds;
    }

    // returns the area (approx.) between the path and the straight line between the start and end points;
    public double Area()
    {
        double sum = 0;
        if (mouseDataPoints.Count < 2)
        {
            return sum;
        }

        double a = new double();
        double c = new double();
        double b = new double();
        var startPoint = mouseDataPoints[0];
        var endPoint = mouseDataPoints[mouseDataPoints.Count - 1];
        ICogMath.GetABC(startPoint.x, startPoint.y, endPoint.x, endPoint.y, ref a, ref b, ref c);
        var prevProjectedPoint = mouseDataPoints[0];
        //Debug.Log(mouseDataPoints.Count);
        foreach (MouseData point in mouseDataPoints)
        {
            double x1 = new double();
            double y1 = new double();
            ICogMath.GetProjection(a, b, c, point.x, point.y, ref x1, ref y1);

            sum += ICogMath.Dist(point.x, point.y, x1, y1) * ICogMath.Dist(x1, y1, prevProjectedPoint.x, prevProjectedPoint.y);
            prevProjectedPoint.x = x1;
            prevProjectedPoint.y = y1;
        }

        sum /= ICogMath.Dist(startPoint.x, startPoint.y, endPoint.x, endPoint.y);

        return sum;
    }

    public void ClearDataPoints()
    {
        mouseDataPoints.Clear();
    }

}
