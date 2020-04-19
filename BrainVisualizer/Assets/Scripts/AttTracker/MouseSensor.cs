﻿using UnityEngine;
using System.Collections.Generic;

public class MouseSensor : MonoBehaviour, IAttentionSensor
{
    public Color colorHighAttention;
    public Color colorLowAttention;
    public Color colorStartEnd;
    public GameObject linePath;
    public GameObject lineStartEnd;
    public float lineWidth;

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

    public void UpdateMouse(float newx, float newy)
    {
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
        if (mouseDataPoints.Count - i < 2)
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

        var v1 = Velocity(i);
        var v2 = Velocity(i + 1);
        var dt = mouseDataPoints[mouseDataPoints.Count - i - 1].t - mouseDataPoints[mouseDataPoints.Count - i - 2].t;

        return (v1 - v2) / dt.TotalMilliseconds;
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
        foreach (MouseData point in mouseDataPoints)
        {
            double x1 = new double();
            double y1 = new double();
            ICogMath.GetProjection(a, b, c, point.x, point.y, ref x1, ref y1);

            sum += ICogMath.Dist(point.x, point.y, x1, y1) * ICogMath.Dist(x1, y1, prevProjectedPoint.x, prevProjectedPoint.y);
            prevProjectedPoint.x = x1;
            prevProjectedPoint.y = y1;
        }

        return sum;
    }

    public double AreaNorm()
    {
        if (mouseDataPoints.Count < 2)
        {
            return 0;
        }

        var startPoint = mouseDataPoints[0];
        var endPoint = mouseDataPoints[mouseDataPoints.Count - 1];
        var lineDist = ICogMath.Dist(startPoint.x, startPoint.y, endPoint.x, endPoint.y);

        return Area() / (lineDist * lineDist);
    }

    public double Path()
    {
        if (mouseDataPoints.Count < 2)
        {
            return 0;
        }

        double res = 0;
        var prevPoint = mouseDataPoints[0];
        foreach (MouseData point in mouseDataPoints)
        {
            res += ICogMath.Dist(point.x, point.y, prevPoint.x, prevPoint.y);
            prevPoint = point;
        }

        return res;
    }

    public double PathNorm()
    {
        if (mouseDataPoints.Count < 2)
        {
            return 0;
        }

        var startPoint = mouseDataPoints[0];
        var endPoint = mouseDataPoints[mouseDataPoints.Count - 1];
        var lineDist = ICogMath.Dist(startPoint.x, startPoint.y, endPoint.x, endPoint.y);

        return Path() / lineDist;
    }


    public void ClearDataPoints()
    {
        mouseDataPoints.Clear();
    }


    // unity only
    public void DrawPath(bool isHighAttention) // , Vector3 startWin, Vector3 endWin)
    {
        var startWinX = transform.position.x - transform.lossyScale.x / 2;
        var startWinY = transform.position.y - transform.lossyScale.y / 2;
        var endWinX = transform.position.x + transform.lossyScale.x / 2;
        var endWinY = transform.position.y + transform.lossyScale.y / 2;

        if (mouseDataPoints.Count < 2)
            return;

        LineRenderer lr = linePath.GetComponent<LineRenderer>();

        lr.material.color = isHighAttention ? colorHighAttention : colorLowAttention;

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;


        var maxx = mouseDataPoints[0].x;
        var minx = mouseDataPoints[0].x;
        var maxy = mouseDataPoints[0].y;
        var miny = mouseDataPoints[0].y;
        foreach (MouseData point in mouseDataPoints)
        {
            if (point.x > maxx) maxx = point.x;
            if (point.x < minx) minx = point.x;
            if (point.y > maxy) maxy = point.y;
            if (point.y < miny) miny = point.y;
        }
        var dx = maxx - minx;
        var dy = maxy - miny;
        var xscale = endWinX - startWinX;
        var yscale = endWinY - startWinY;

        Vector3[] allVertices = new Vector3[mouseDataPoints.Count];
        int i = 0;
        foreach (MouseData point in mouseDataPoints)
        {
            allVertices[i].x = (dx != 0) ? (float)((point.x - minx) / dx) * xscale + startWinX : startWinX;
            allVertices[i].y = (dy != 0) ? (float)((point.y - miny) / dy) * yscale + startWinY : startWinY;
            allVertices[i].z = transform.position.z;
            i++;
        }

        lr.positionCount = allVertices.Length;
        lr.SetPositions(allVertices);
    }




    public void DrawStartEnd() // Vector3 startWin, Vector3 endWin)
    {
        var startWinX = transform.position.x - transform.lossyScale.x / 2;
        var startWinY = transform.position.y - transform.lossyScale.y / 2;
        var endWinX = transform.position.x + transform.lossyScale.x / 2;
        var endWinY = transform.position.y + transform.lossyScale.y / 2;

        if (mouseDataPoints.Count < 2)
            return;

        LineRenderer lr = lineStartEnd.GetComponent<LineRenderer>();

        lr.material.color = colorStartEnd;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        var maxx = mouseDataPoints[0].x;
        var minx = mouseDataPoints[0].x;
        var maxy = mouseDataPoints[0].y;
        var miny = mouseDataPoints[0].y;
        foreach (MouseData point in mouseDataPoints)
        {
            if (point.x > maxx) maxx = point.x;
            if (point.x < minx) minx = point.x;
            if (point.y > maxy) maxy = point.y;
            if (point.y < miny) miny = point.y;
        }
        var dx = maxx - minx;
        var dy = maxy - miny;
        var xscale = endWinX - startWinX;
        var yscale = endWinY - startWinY;

        Vector3[] startEndLine = new Vector3[2];

        startEndLine[0].x = (dx != 0) ? (float)((mouseDataPoints[0].x - minx) / dx) * xscale + startWinX : startWinX;
        startEndLine[0].y = (dy != 0) ? (float)((mouseDataPoints[0].y - miny) / dy) * yscale + startWinY : startWinY;
        startEndLine[0].z = transform.position.z;
        startEndLine[1].x = (dx != 0) ? (float)((mouseDataPoints[mouseDataPoints.Count - 1].x - minx) / dx) * xscale + startWinX : startWinX;
        startEndLine[1].y = (dy != 0) ? (float)((mouseDataPoints[mouseDataPoints.Count - 1].y - miny) / dy) * yscale + startWinY : startWinY;
        startEndLine[1].z = transform.position.z;

        lr.positionCount = startEndLine.Length;
        lr.SetPositions(startEndLine);
    }
}