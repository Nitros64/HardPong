using Microsoft.Xna.Framework;

namespace HardPong;

// Deteccion pura: indica que limites de la cancha viola el rectangulo de la pelota. No modifica nada.
internal static class CollisionBallWall
{
    public static BallBoundaryContact Detect(Rectangle ballRect, Rectangle bounds)
    {
        bool outLeft = false, outRight = false, outTop = false, outBottom = false;

        if (ballRect.X + ballRect.Width >= bounds.Width)
            outRight = true;
        else if (ballRect.X <= 0)
            outLeft = true;

        if (ballRect.Y + ballRect.Height >= bounds.Height)
            outBottom = true;
        else if (ballRect.Y <= 0)
            outTop = true;

        return new BallBoundaryContact(outLeft, outRight, outTop, outBottom);
    }
}
