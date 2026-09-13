using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Interfaces;

internal interface IBallBoundaryCollision
{
    void Resolve(Ball ball, Rectangle bounds);
}
