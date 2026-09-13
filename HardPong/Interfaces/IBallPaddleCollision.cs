using HardPong.SpriteClass;

namespace HardPong.Interfaces;

internal interface IBallPaddleCollision
{
    void Resolve(Ball ball, Paddle paddle);
}
