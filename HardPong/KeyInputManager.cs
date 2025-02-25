using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace HardPong;

internal class KeyInputManager
{
    private KeyboardState _oldKeyState;
    private KeyboardState _newKeyState;
    private readonly List<Keys> _triggers = [];
    private bool _firstTime = true;

    public void AddTriggerKeys(params Keys[] newTriggers)
    {
        if (newTriggers.Length <= 0) return;
        for(ushort i = 0; i < newTriggers.Length; ++i)
            _triggers.Add(newTriggers[i]);
    }

    public void Begin() {
        var currentKeyState = Keyboard.GetState(); // Solo una llamada
        if (_firstTime) {
            if (_triggers.Any(currentKeyState.IsKeyDown))
                return;

            _firstTime = false;
        }
        _newKeyState = Keyboard.GetState();            
    }

    public bool CheckPressedKey(Keys key) {
        return _oldKeyState.IsKeyUp(key) && _newKeyState.IsKeyDown(key);
    }

    public void End(){
        _oldKeyState = _newKeyState;
    }

    public void Exit(){
        _firstTime = true;
    }  
}