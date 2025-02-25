using Microsoft.Xna.Framework;
using HardPong.Interfaces;
using HardPong.SpriteClass;

namespace HardPong;

internal class CollisionDetector(
    ISpriteCollision scBallPaddle,
    ISpriteCollisionEnvironment scwBallWall,
    ISpriteCollisionEnvironment scwPaddleWall)
{
    //Collition Interfaces
    //Ball and Paddle Handler
    //Ball Handler
    //Paddle Handler

    public void collision_ball_paddle(Ball refBall,Paddle refPaddle) {
        scBallPaddle.SpriteCollision(refBall,refPaddle);
    }

    public void collision_ball_wall(Ball refBall,Rectangle clientBounds) {
        scwBallWall.SpriteCollisionEnviroment(refBall,clientBounds);
    }

    public void collision_paddle_wall(Paddle refPaddle,Rectangle clientBounds)
    {
        scwPaddleWall.SpriteCollisionEnviroment(refPaddle,clientBounds);
    }
}