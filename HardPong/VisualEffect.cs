using Microsoft.Xna.Framework;

namespace HardPong;

public class PositionChanger(Vector2 posVector1, Vector2 posVector2)
{
    public Vector2 GetFirst() {
        return posVector1;
    }
    public Vector2 VectorSwitch(bool flag)
    {
        return flag ? posVector2 : posVector1;
    }
}