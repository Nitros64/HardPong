using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Interfaces;

internal interface IPaddleBoundaryCollision
{
    void Resolve(Paddle paddle, Rectangle bounds);
}
