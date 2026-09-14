using System;
using Microsoft.Xna.Framework.Input;
using static HardPong.GameEnum;

namespace HardPong;

// Lee el teclado y traduce las pulsaciones a GameAction.
// No conoce pantallas, sprites ni audio.
internal class GameInputReader
{
    private readonly KeyInputManager _inputManager;

    public GameInputReader()
    {
        _inputManager = new KeyInputManager();
        _inputManager.AddTriggerKeys(Keys.Enter, Keys.Escape);
    }

    public GameAction ReadAction(GameStates state, Func<int> exitMenuSelection)
    {
        if (state == GameStates.ExitMenu)
            return GameInputMapper.FromExitMenu(exitMenuSelection());

        _inputManager.Begin();
        GameAction action = GameInputMapper.FromGameplay(state,
            _inputManager.CheckPressedKey(Keys.Enter),
            _inputManager.CheckPressedKey(Keys.Escape));
        _inputManager.End();
        return action;
    }

    // Rearma el periodo de gracia (evita bordes fantasma entre pantallas)
    public void ResetGracePeriod() => _inputManager.Exit();
}
