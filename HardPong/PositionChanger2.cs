using Microsoft.Xna.Framework;

namespace HardPong;

public class PositionChanger2(Point posVector1, Point posVector2)
{
    public Vector2 GetFirst()
    {
        return new Vector2(posVector1.X, posVector1.Y);
    }
    public Vector2 VectorSwitch(bool flag)
    {
        return flag ? new Vector2(posVector2.X, posVector2.Y) : new Vector2(posVector1.X, posVector1.Y);
    }
}