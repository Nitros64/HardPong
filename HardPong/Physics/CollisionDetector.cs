using Microsoft.Xna.Framework;

namespace HardPong;

// Orquesta el orden: primero detectar (puro), luego responder.
// El orden de las colisiones lo fija SpriteManager en su Update.
internal class CollisionDetector
{
    private readonly CollisionResponse _response = new();

    public CollisionEvents ResolveBallPaddle(Ball ball, Paddle paddle)
    {
        BallPaddleContact contact = CollisionBallPaddle.Detect(ball.CollisionRect, paddle.CollisionRect);
        return contact.Hit
            ? _response.ResolveBallPaddle(ball, paddle, contact)
            : CollisionEvents.None;
    }

    public BallBoundaryOutcome ResolveBallBoundary(Ball ball, Rectangle bounds)
    {
        BallBoundaryContact contact = CollisionBallWall.Detect(ball.CollisionRect, bounds);
        return contact.OutLeft || contact.OutRight || contact.OutTop || contact.OutBottom
            ? _response.ResolveBallBoundary(ball, contact, bounds)
            : default;
    }

    public void ResolvePaddleBoundary(Paddle paddle, Rectangle bounds)
    {
        if (CollisionPaddleWall.Detect(paddle.CollisionRect, bounds))
            _response.ResolvePaddleBoundary(paddle, bounds);
    }
}
