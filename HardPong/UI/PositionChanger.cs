using Microsoft.Xna.Framework;

namespace HardPong;

public class PositionChanger(Vector2 posVector1, Vector2 posVector2)
{
    public PositionChanger(Point position1, Point position2)
        : this(new Vector2(position1.X, position1.Y), new Vector2(position2.X, position2.Y))
    {
    }

    public Vector2 GetFirst() {
        return posVector1;
    }
    public Vector2 VectorSwitch(bool flag)
    {
        return flag ? posVector2 : posVector1;
    }
}
