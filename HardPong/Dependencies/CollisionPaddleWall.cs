using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;
internal class CollisionPaddleWall : IPaddleBoundaryCollision
{
    public void Resolve(Paddle paddle, Rectangle bounds)
    {
        if (paddle.PositionY <= 0)
            paddle.PositionY = 0;
        if (paddle.PositionY + paddle.SpriteFrameSize.Y >= bounds.Height)
            paddle.PositionY = bounds.Height - paddle.SpriteFrameSize.Y;
    }
}
