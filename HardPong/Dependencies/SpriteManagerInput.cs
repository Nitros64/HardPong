using HardPong.Interfaces;
using Microsoft.Xna.Framework.Input;
using static HardPong.GameEnum;

namespace HardPong.Dependencies;

// Lee el teclado y traduce las pulsaciones a GameAction via GameInputMapper.
// La ejecucion de acciones vive aqui hasta que la navegacion de pantallas
// tenga su propio coordinador (segunda mitad del paso 7).
class SpriteManagerInput : IKeyboardInput
{
    //Menu Simple
    private readonly SpriteManager _spritemanager;
    private readonly KeyInputManager _inputManager;
    private readonly Game1 _refGame1;

    public SpriteManagerInput(SpriteManager sm, Game1 refGame1)
    {
        _spritemanager = sm;
        _refGame1 = refGame1;
        _inputManager = new KeyInputManager();
        _inputManager.AddTriggerKeys(Keys.Enter, Keys.Escape);
    }

    public void CheckKeyboardInput()
    {
        GameStateController gsc = _spritemanager.getGameStateController;
        GameStates state = gsc.GetGameState();

        if (state != GameStates.ExitMenu)
        {
            _inputManager.Begin();
            GameAction action = GameInputMapper.FromGameplay(state,
                _inputManager.CheckPressedKey(Keys.Enter),
                _inputManager.CheckPressedKey(Keys.Escape));
            _inputManager.End();
            Execute(action);
        }
        else
        {
            Execute(GameInputMapper.FromExitMenu(_spritemanager.EscapeMenu.Update()));
        }
    }

    private void Execute(GameAction action)
    {
        GameStateController gsc = _spritemanager.getGameStateController;
        switch (action)
        {
            case GameAction.StartMatch:
                gsc.Play();
                break;
            case GameAction.Pause:
                gsc.Pause();
                break;
            case GameAction.Resume:
                gsc.Resume();
                break;
            case GameAction.PrepareNextRound:
                gsc.PrepareNextRound();
                if (_spritemanager.IsMatchOver)
                    _spritemanager.Begin();
                else{
                    _spritemanager.reset_soundEffects();
                    _spritemanager.ResetPosition();
                    _spritemanager.StartNextRound();
                }
                break;
            case GameAction.OpenExitMenu:
                gsc.OpenExitMenu();
                _inputManager.Exit();
                break;
            case GameAction.ContinueGame:
                gsc.ResumeFromExitMenu();
                _spritemanager.EscapeMenu.InputManager.Exit();
                break;
            case GameAction.ReturnToMainMenu:
                _spritemanager.Game.Components.Remove(_spritemanager);
                _spritemanager.Game.Components.Add(_refGame1.GetMenuPong);
                _spritemanager.EscapeMenu.InputManager.Exit();
                _inputManager.Exit();
                break;
            case GameAction.QuitGame:
                _spritemanager.Game.Exit();
                break;
        }
    }
}
