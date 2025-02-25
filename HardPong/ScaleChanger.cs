using System;

namespace HardPong;

public class ScaleChanger(int time, float scale1, float scale2)
{
    private const double ToleranceD = 0.0001;
    private float _scale = scale1;
    private readonly float _scale1 = scale1;
    private int _cont;

    public float VisualEffect()
    {
        if (++_cont <= time)
            return _scale;
        ScaleSwitch();
        _cont = 0;
        return _scale;
    }
    private void ScaleSwitch()
    {
        _scale = Math.Abs(_scale - _scale1) < ToleranceD ? scale2 : _scale1;
    }
}