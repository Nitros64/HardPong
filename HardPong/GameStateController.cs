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

    public void SetGameState(GameEnum.GameStates mygameState)
    {
        _currentState = mygameState;
    }

    public GameEnum.GameStates GetGameState()
    {
        return _currentState;
    }
    
    public void SetGameOldState(GameEnum.GameStates myGameOldState)
    {
        _oldState = myGameOldState;
    }

    public GameEnum.GameStates GetGameOldState()
    {
        return _oldState;
    }        
}