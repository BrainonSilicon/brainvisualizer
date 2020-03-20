
using System;

public class ICogMath
{

    // the distance betwee Point P(x,y) and Line (ax + by + c = 0)
    static public double Dist(double x, double y, double a, double b, double c)
    {
        double numerator = Math.Abs(a * x + b * y + c);
        double denominator = Math.Sqrt(a * a + b * b);
        return numerator / denominator;
    }

    // distance between two points (x0,y0), (x1,y1)
    static public double Dist(double x0, double y0, double x1, double y1)
    {
        return Math.Sqrt((y0 - y1) * (y0 - y1) + (x0 - x1) * (x0 - x1));
    }

    // Given P1(x0, y0) and P2(x1,y1)
    // calculates the A and C in the Formula Ax + Y + c=0
    static public void GetABC(double x0, double y0, double x1, double y1, ref double a, ref double b, ref double c)
    {
        if (Math.Abs(x1 - x0) > Math.Abs(y1 - y0))
        {
            b = 1;
            a = -(y0 - y1) / (x0 - x1);
            c = (-y0 - a * x0);
        }
        else
        {
            a = 1;
            b = -(x0 - x1) / (y0 - y1);
            c = (-x0 - b * y0);
        }

    }

    // given a line in the form (ax + by + c = 0) and a point (x0,y0)
    // return the projection x1,y1 on the line
    static public void GetProjection(double a, double b, double c, double x0, double y0, ref double x1, ref double y1)
    {
        var norm = a * a + b * b;
        x1 = (b * (b * x0 - a * y0) - a * c) / norm;
        y1 = (a * (a * y0 - b * x0) - b * c) / norm;
    }
}
