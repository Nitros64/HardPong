using HardPong.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace HardPong.Dependencies;

internal class KeyboardReader : IKeyboardReader
{
    public KeyboardState Capture() => Keyboard.GetState();
}
