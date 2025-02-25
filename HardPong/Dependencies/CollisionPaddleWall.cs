using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;
class CollisionPaddleWall : ISpriteCollisionEnvironment
{
    public void SpriteCollisionEnviroment(Sprite s1, Rectangle clientBounds)
    {
        if (s1.PositionY <= 0)
            s1.PositionY = 0;
        if (s1.PositionY + s1.SpriteFrameSize.Y >= clientBounds.Height)
            s1.PositionY = clientBounds.Height - s1.SpriteFrameSize.Y;
    }
}
