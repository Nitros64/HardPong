using Microsoft.Xna.Framework;

namespace HardPong;

// Coordina la deteccion de contactos y la aplicacion de sus respuestas fisicas.
// MatchSession determina el orden en que se resuelven las colisiones.
internal class CollisionResolver
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
