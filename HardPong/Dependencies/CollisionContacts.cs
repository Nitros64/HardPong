using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;

// Hechos geometricos detectados; no contienen comportamiento.
internal readonly record struct BallPaddleContact(
    bool Hit,
    Rectangle BallRect,
    Rectangle PaddleRect,
    float Angle,
    float Slope);

internal readonly record struct BallBoundaryContact(
    bool OutLeft,
    bool OutRight,
    bool OutTop,
    bool OutBottom);
