using Microsoft.Xna.Framework.Input;

namespace HardPong.Interfaces;

internal interface IKeyboardReader
{
    bool IsKeyDown(Keys key);
}
