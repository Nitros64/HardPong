using System;
using HardPong.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace HardPong.Dependencies;

internal class PaddleInputMovement : IPaddleController
{
    private readonly IKeyboardReader _keyboard;
    private readonly Keys _keyUp, _keyDown;

    public PaddleInputMovement(IKeyboardReader keyboard, Keys up, Keys down)
    {
        _keyboard = keyboard ?? throw new ArgumentNullException(nameof(keyboard));
        _keyUp = up;
        _keyDown = down;
    }

    public float ReadMovementAxis()
    {
        float axis = 0f;
        if (_keyboard.IsKeyDown(_keyUp))
            axis -= 1f;
        if (_keyboard.IsKeyDown(_keyDown))
            axis += 1f;
        return axis;
    }
}
