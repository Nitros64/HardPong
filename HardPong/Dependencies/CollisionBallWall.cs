using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;

// Deteccion pura: indica que limites de la cancha viola la pelota. No modifica nada.
internal static class CollisionBallWall
{
    public static BallBoundaryContact Detect(Ball ball, Rectangle bounds)
    {
        Rectangle rectball = ball.CollisionRect;

        bool outLeft = false, outRight = false, outTop = false, outBottom = false;

        if (rectball.X + rectball.Width >= bounds.Width)
            outRight = true;
        else if (rectball.X <= 0)
            outLeft = true;

        if (rectball.Y + rectball.Height >= bounds.Height)
            outBottom = true;
        else if (rectball.Y <= 0)
            outTop = true;

        return new BallBoundaryContact(outLeft, outRight, outTop, outBottom);
    }
}
