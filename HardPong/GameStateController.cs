using HardPong.Interfaces;

namespace HardPong;
public class GameStateController : IGameState
{
    //States     
    private GameEnum.GameStates _currentState, _oldState;

    public GameStateController()
    {
        Ready();
        _oldState = _currentState;
    }

    public void Ready()
    {
        _currentState = GameEnum.GameStates.Ready;
    }       

    public void Play()
    {
        if (_currentState == GameEnum.GameStates.Ready)
            _currentState = GameEnum.GameStates.Playing;
    }

    public void Pause()
    {
        _currentState = _currentState switch
        {
            GameEnum.GameStates.Playing => GameEnum.GameStates.Paused,
            GameEnum.GameStates.Paused => GameEnum.GameStates.Playing,
            _ => _currentState
        };
    }

    public void Stop()
    {
        _currentState = _currentState switch
        {
            GameEnum.GameStates.Playing => GameEnum.GameStates.Stop,
            GameEnum.GameStates.Stop => GameEnum.GameStates.Ready,
            _ => _currentState
        };
    }

    public void OpenExitMenu()
    {
        if (_currentState == GameEnum.GameStates.ExitMenu)
            return;

        _oldState = _currentState;
        _currentState = GameEnum.GameStates.ExitMenu;
    }

    public void ResumeFromExitMenu()
    {
        if (_currentState != GameEnum.GameStates.ExitMenu)
            return;

        _currentState = _oldState;
    }

    public GameEnum.GameStates GetGameState()
    {
        return _currentState;
    }
}