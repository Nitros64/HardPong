using System;
using Microsoft.Xna.Framework.Input;

namespace HardPong;

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
        KeyboardState keyState = _keyboard.Capture(); // una sola captura por lectura
        float axis = 0f;
        if (keyState.IsKeyDown(_keyUp))
            axis -= 1f;
        if (keyState.IsKeyDown(_keyDown))
            axis += 1f;
        return axis;
    }
}
