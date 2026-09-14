using System;
using Microsoft.Xna.Framework;

namespace HardPong;

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

// Resultado de resolver un contacto con los limites: audio y, si lo hubo, quien anota.
internal readonly record struct BallBoundaryOutcome(CollisionSound Sound, PlayerId Scorer);

// Eventos que la respuesta comunica; la coordinacion de la partida reproduce el audio.
[Flags]
internal enum CollisionSound
{
    None = 0,
    Paddle = 1 << 0,
    Score = 1 << 1,
    Wall = 1 << 2
}
