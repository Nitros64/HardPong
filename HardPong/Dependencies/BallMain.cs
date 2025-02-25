using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;
class BallMain : ISpriteAutomaticMovement
{
    public void AutomaticMovement(Sprite s)
    {
        s.SpritePosition += s.Direction;
    }

    public void AutomaticMovement(Sprite s, Rectangle clientBounds)
    {
        s.SpritePosition += s.Direction;
    }
}