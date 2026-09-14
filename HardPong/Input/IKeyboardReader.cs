using Microsoft.Xna.Framework.Input;

namespace HardPong;

internal interface IKeyboardReader
{
    // Una captura por lectura; las teclas de una misma operacion se consultan sobre ella.
    KeyboardState Capture();
}
