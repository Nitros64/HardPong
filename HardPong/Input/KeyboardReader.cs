using Microsoft.Xna.Framework.Input;

namespace HardPong;

internal class KeyboardReader : IKeyboardReader
{
    public KeyboardState Capture() => Keyboard.GetState();
}
