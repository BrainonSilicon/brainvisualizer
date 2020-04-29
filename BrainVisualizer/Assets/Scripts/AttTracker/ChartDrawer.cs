using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartDrawer : MonoBehaviour
{
    public Color lineColor;
    public float lineWidth = 0.3f;
    public int maxDataPoints = 10;
    public UnityEngine.UI.Text valChange;

    private float lastValue = 0;

    public struct DataPoint
    {
        public double x;
        public double y;
    }

    public List<DataPoint> LineDataPoints = new List<DataPoint>();

    public void Start()
    {
        DataPoint point = new DataPoint();
        point.x = 0;
        point.y = 0;

        LineDataPoints.Add(point);
    }

    public void DrawLine()
    {
        if (LineDataPoints.Count < 2)
            return;


        var startWinX = transform.position.x - transform.lossyScale.x / 2;
        var startWinY = transform.position.y - transform.lossyScale.y / 2;
        var endWinX = transform.position.x + transform.lossyScale.x / 2;
        var endWinY = transform.position.y + transform.lossyScale.y / 2;


        LineRenderer lr = GetComponent<LineRenderer>();

        lr.material.color = lineColor;

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;


        var maxx = LineDataPoints[0].x;
        var minx = LineDataPoints[0].x;
        var maxy = LineDataPoints[0].y;
        var miny = LineDataPoints[0].y;
        foreach (DataPoint point in LineDataPoints)
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

        Vector3[] allVertices = new Vector3[LineDataPoints.Count];
        int i = 0;
        foreach (DataPoint point in LineDataPoints)
        {
            allVertices[i].x = (dx != 0) ? (float)((point.x - minx) / dx) * xscale + startWinX : startWinX;
            allVertices[i].y = (dy != 0) ? (float)((point.y - miny) / dy) * yscale + startWinY : startWinY;
            allVertices[i].z = transform.position.z;
            i++;
        }

        lr.positionCount = allVertices.Length;
        lr.SetPositions(allVertices);
    }

    public void AddDataPoint(float value)
    {
        DataPoint point = new DataPoint();

        point.x = LineDataPoints[LineDataPoints.Count - 1].x + 1;
        point.y = value;

        LineDataPoints.Add(point);

        if (LineDataPoints.Count > maxDataPoints)
        {
            LineDataPoints.RemoveAt(0);
        }

        valChange.text = (value - lastValue).ToString("N1");
        lastValue = value;
    }
}
