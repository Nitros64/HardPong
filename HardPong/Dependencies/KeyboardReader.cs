using HardPong.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace HardPong.Dependencies;

internal class KeyboardReader : IKeyboardReader
{
    public bool IsKeyDown(Keys key) => Keyboard.GetState().IsKeyDown(key);
}
