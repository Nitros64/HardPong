using Microsoft.Xna.Framework;

namespace HardPong;

// Deteccion pura: indica si el rectangulo sale de los limites verticales.
internal static class CollisionPaddleWall
{
    public static bool Detect(Rectangle paddleRect, Rectangle bounds)
    {
        return paddleRect.Y <= 0
            || paddleRect.Y + paddleRect.Height >= bounds.Height;
    }
}
