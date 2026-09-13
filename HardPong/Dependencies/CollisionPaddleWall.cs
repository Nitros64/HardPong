using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;

// Deteccion pura: indica si la pala sale de los limites verticales.
internal static class CollisionPaddleWall
{
    public static bool Detect(Paddle paddle, Rectangle bounds)
    {
        return paddle.PositionY <= 0
            || paddle.PositionY + paddle.SpriteFrameSize.Y >= bounds.Height;
    }
}
