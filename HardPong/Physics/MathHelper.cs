using System;

namespace HardPong;

internal abstract class MathHelper
{
    public static double Angle(float x1, float y1, float x2, float y2) {
        double radians = Math.Atan(CalculateSlope(x2 - x1, y2 - y1));
        return radians * (180 / Math.PI);
    }

    public static double Angle(float x1, float y1, float x2, float y2, out float m)
    {
        m = CalculateSlope(x2 - x1, y2 - y1);
        double radians = Math.Atan(m);
        double angle = radians * (180 / Math.PI);

        if (angle < 0) angle *= -1;
        if (m < 0) m *= -1;

        return angle;
    }

    private static float CalculateSlope(float deltaX, float deltaY)
    {
        if (deltaX != 0f)
            return deltaY / deltaX;

        // Sin separacion entre centros, usamos un impacto frontal.
        if (deltaY == 0f)
            return 0f;

        // Saturamos la pendiente vertical; conserva el angulo de 90 grados
        // y permite que Ball aplique sus limites de velocidad habituales.
        return deltaY > 0f ? float.MaxValue : -float.MaxValue;
    }
}
