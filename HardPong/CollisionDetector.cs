using Microsoft.Xna.Framework;
using HardPong.Interfaces;
using HardPong.SpriteClass;

namespace HardPong;

internal class CollisionDetector(
    IBallPaddleCollision ballPaddle,
    IBallBoundaryCollision ballBoundary,
    IPaddleBoundaryCollision paddleBoundary)
{
    //Collision Interfaces
    //Ball and Paddle Handler
    //Ball Handler
    //Paddle Handler

    public void ResolveBallPaddle(Ball ball, Paddle paddle) {
        ballPaddle.Resolve(ball, paddle);
    }

    public void ResolveBallBoundary(Ball ball, Rectangle bounds) {
        ballBoundary.Resolve(ball, bounds);
    }

    public void ResolvePaddleBoundary(Paddle paddle, Rectangle bounds)
    {
        paddleBoundary.Resolve(paddle, bounds);
    }
}