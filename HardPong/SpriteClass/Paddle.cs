using System;
using HardPong.Interfaces;
using Microsoft.Xna.Framework;

namespace HardPong.SpriteClass;

// Estado de una pala: posicion, tamano y desplazamiento vertical.
// No conoce texturas ni teclado; se mueve segun su IPaddleController.
internal class Paddle
{
    public const int BrickWidth = 15;
    public const int BrickHeight = 60;
    public const float BrickSpeedY = 5;

    private readonly IPaddleController _controller;
    private Vector2 _position;
    private Vector2 _oldPosition;

    public Paddle(Vector2 spawn, IPaddleController controller)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        _position = spawn;
        _oldPosition = spawn;
    }

    public Vector2 Position => _position;
    public Rectangle CollisionRect => new((int)_position.X, (int)_position.Y, BrickWidth, BrickHeight);

    public void Update()
    {
        float axis = _controller.ReadMovementAxis();
        _oldPosition = _position; // las colisiones utilizan OldPosition
        _position.Y += axis * BrickSpeedY;
    }

    public void SetPosition(float x, float y) => _position = new Vector2(x, y);

    public void GetBackToOldPosition() => _position = _oldPosition;
}
