using System;

namespace HardPong;

internal abstract class MathHelper
{
    public static double Angle(float x1, float y1, float x2, float y2) {                        
        double radians = Math.Atan((y2 - y1) / (x2 - x1));
        return radians * (180 / Math.PI);
    }

    public static double Angle(float x1, float y1, float x2, float y2, out float m)
    {
        m = (y2 - y1) / (x2 - x1);
        double radians = Math.Atan(m);
        double angle = radians * (180 / Math.PI);

        if (angle < 0) angle *= -1;
        if (m < 0) m *= -1;

        return angle;
    }
}