using System;
using Microsoft.Xna.Framework;

namespace HardPong;

// Hechos geometricos detectados; no contienen comportamiento.
internal readonly record struct BallPaddleContact(
    bool Hit,
    Rectangle BallRect,
    Rectangle PaddleRect,
    float Angle,
    float Slope,
    bool IsBorderContact);

internal readonly record struct BallBoundaryContact(
    bool OutLeft,
    bool OutRight,
    bool OutTop,
    bool OutBottom);

// Hechos del contacto con los limites y, si lo hubo, quien anota.
internal readonly record struct BallBoundaryOutcome(CollisionEvents Events, PlayerId Scorer);

// Hechos de la simulacion, independientes de su presentacion.
[Flags]
internal enum CollisionEvents
{
    None = 0,
    PaddleHit = 1 << 0,
    PointScored = 1 << 1,
    WallHit = 1 << 2
}
