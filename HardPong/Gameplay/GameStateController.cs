
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
        if (_currentState == GameEnum.GameStates.Playing)
            _currentState = GameEnum.GameStates.Paused;
    }

    public void Resume()
    {
        if (_currentState == GameEnum.GameStates.Paused)
            _currentState = GameEnum.GameStates.Playing;
    }

    // Termina la ronda en curso (se anoto un punto)
    public void EndRound()
    {
        if (_currentState == GameEnum.GameStates.Playing)
            _currentState = GameEnum.GameStates.Stop;
    }

    // Prepara la siguiente ronda tras la celebracion
    public void PrepareNextRound()
    {
        if (_currentState == GameEnum.GameStates.Stop)
            _currentState = GameEnum.GameStates.Ready;
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