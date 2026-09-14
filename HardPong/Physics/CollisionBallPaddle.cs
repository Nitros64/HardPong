using Microsoft.Xna.Framework;

namespace HardPong;

// Deteccion pura: no modifica nada ni reproduce sonido.
internal static class CollisionBallPaddle
{
    public static BallPaddleContact Detect(Rectangle ballRect, Rectangle paddleRect)
    {
        if (!ballRect.Intersects(paddleRect))
            return default;

        float m;
        float angle = (float)
            MathHelper.Angle(ballRect.X + ballRect.Width / 2, ballRect.Y + ballRect.Height / 2,
                        paddleRect.X + paddleRect.Width / 2, paddleRect.Y + paddleRect.Height / 2,
                        out m);

        return new BallPaddleContact(true, ballRect, paddleRect, angle, m);
    }
}
