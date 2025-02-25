using HardPong.Interfaces;
using Microsoft.Xna.Framework.Input;
using static HardPong.SpriteClass.Ball;
using static HardPong.GameEnum;

namespace HardPong.Dependencies;
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
        GameStates mystate = _spritemanager.getGameStateController.GetGameState();
        GameStateController gsc = _spritemanager.getGameStateController;

        if (mystate != GameStates.ExitMenu)           
            check_key_enter_and_escape(); //Se chequea por la tecla Enter o Escape            
        else if (mystate == GameStates.ExitMenu) {
            switch (_spritemanager.EscapeMenu.Update()){               
                case 0: case 1://Escape //Enter Continue
                    gsc.SetGameState(gsc.GetGameOldState());
                    _spritemanager.EscapeMenu.InputManager.Exit();
                    break;
                case 2: //Return Main Menu
                    _spritemanager.Game.Components.Remove(_spritemanager);
                    _spritemanager.Game.Components.Add(_refGame1.GetMenuPong);
                    _spritemanager.EscapeMenu.InputManager.Exit();
                    _inputManager.Exit();
                    break;
                case 3:
                    _spritemanager.Game.Exit();
                    break;
            }
        } 
    }

    private void check_key_enter_and_escape() {           
        // Se chequea si el usuario a presionado Enter durante el estado Fase Playing
        // lo cual hace que el juego entre en estado de Pausa
        GameStateController gsc = _spritemanager.getGameStateController;
        _inputManager.Begin();
        if (_inputManager.CheckPressedKey(Keys.Enter))
        {
            switch (gsc.GetGameState())
            {
                case GameStates.Ready:
                    gsc.SetGameState(GameStates.Playing);
                    break;
                case GameStates.Paused:
                    gsc.Pause();//toogle betweent Playing and Paused
                    break;
                case GameStates.Playing:
                    gsc.Pause();//toogle betweent Playing and Paused
                    break;
                case GameStates.Stop:
                    gsc.Stop();//toogle
                    if (_spritemanager.PlayerWinner >= 1)
                        _spritemanager.Begin();                        
                    else{
                        _spritemanager.reset_soundEffects();
                        _spritemanager.ResetPosition();
                        _spritemanager.GetBall().Winner = PlayerNumber.NoOne;
                    }
                    break;
            }
        }
        else if (_inputManager.CheckPressedKey(Keys.Escape))
        {
            gsc.SetGameOldState(gsc.GetGameState());
            gsc.SetGameState(GameStates.ExitMenu);
            _inputManager.Exit();             
        }
        _inputManager.End();
    }
}