using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HardPong;

public class ColorChanger
{
    private readonly int _time;
    private readonly List<Color> _setColors;
    private int _colorIndex;
    private int _cont;

    public ColorChanger(int time = 0, params Color[] setColors)
    {
        if (setColors is null || setColors.Length == 0)
            throw new ArgumentException("ColorChanger requiere al menos un color.", nameof(setColors));

        _time = time;
        _setColors = [.. setColors];
    }

    public Color Current => _setColors[_colorIndex];

    // Avanza el ciclo; llamar desde Update, no desde Draw
    public void Advance()
    {
        if (++_cont <= _time)
            return;
        ColorSwitch();
        _cont = 0;
    }

    private void ColorSwitch()
    {
        _colorIndex = (_colorIndex + 1) % _setColors.Count;
    }
}