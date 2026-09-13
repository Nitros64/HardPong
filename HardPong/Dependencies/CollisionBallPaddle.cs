using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;

// Deteccion pura: no modifica nada ni reproduce sonido.
internal static class CollisionBallPaddle
{
    public static BallPaddleContact Detect(Ball ball, Paddle paddle)
    {
        Rectangle rectBall = ball.CollisionRect;
        Rectangle rectPaddle = paddle.CollisionRect;

        if (!rectBall.Intersects(rectPaddle))
            return default;

        float m;
        float angle = (float)
            MathHelper.Angle(rectBall.X + rectBall.Width / 2, rectBall.Y + rectBall.Height / 2,
                        rectPaddle.X + rectPaddle.Width / 2, rectPaddle.Y + rectPaddle.Height / 2,
                        out m);

        return new BallPaddleContact(true, rectBall, rectPaddle, angle, m);
    }
}
