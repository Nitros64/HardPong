using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HardPong.Dependencies;

class PaddleInputMovement : IKeyboardInput
{
    private readonly Paddle _paddle;
    private Keys _keyUp, _keyDown;

    public PaddleInputMovement(Paddle paddle,Keys up, Keys down)
    {
        this._paddle = paddle;
        SetKeys(up, down);
    }

    public PaddleInputMovement(Paddle paddle)
    {
        _paddle = paddle;
    }

    public void CheckKeyboardInput()
    {
        Vector2 inputDirection = KeyUpdate();
        _paddle.OldPosition = _paddle.SpritePosition;
        _paddle.SpritePosition += inputDirection * _paddle.Direction;
    }

    private Vector2 KeyUpdate() {
        Vector2 inputDirection = Vector2.Zero;
        if (Keyboard.GetState().IsKeyDown(_keyUp))
            inputDirection.Y -= 1;
        if (Keyboard.GetState().IsKeyDown(_keyDown))
            inputDirection.Y += 1;

        return inputDirection;
    }

    public void SetKeys(Keys up, Keys down) {
        this._keyUp   = up;
        this._keyDown = down;
    }
}
