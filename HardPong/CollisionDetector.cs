using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;

// Orquesta el orden: primero detectar (puro), luego responder.
// El orden de las colisiones lo fija SpriteManager en su Update.
internal class CollisionDetector
{
    private readonly CollisionResponse _response = new();

    public void ResolveBallPaddle(Ball ball, Paddle paddle)
    {
        BallPaddleContact contact = CollisionBallPaddle.Detect(ball, paddle);
        if (contact.Hit)
            _response.ResolveBallPaddle(ball, paddle, contact);
    }

    public void ResolveBallBoundary(Ball ball, Rectangle bounds)
    {
        BallBoundaryContact contact = CollisionBallWall.Detect(ball, bounds);
        if (contact.OutLeft || contact.OutRight || contact.OutTop || contact.OutBottom)
            _response.ResolveBallBoundary(ball, contact, bounds);
    }

    public void ResolvePaddleBoundary(Paddle paddle, Rectangle bounds)
    {
        if (CollisionPaddleWall.Detect(paddle, bounds))
            _response.ResolvePaddleBoundary(paddle, bounds);
    }
}
