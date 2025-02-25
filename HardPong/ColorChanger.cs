using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HardPong;

public class ColorChanger(int time = 0, params Color[] setColors)
{
    private readonly List<Color> _setColors = [..setColors];
    private int _colorIndex;
    private int _cont;

    public Color VisualEffect() {
        if (++_cont <= time) return _setColors[_colorIndex];
        ColorSwitch();
        _cont = 0;
        return _setColors[_colorIndex];
    }

    private void ColorSwitch()
    {
        _colorIndex = (_colorIndex + 1) % _setColors.Count;
    }
}