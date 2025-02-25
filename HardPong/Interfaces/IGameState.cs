using static HardPong.GameEnum;

namespace HardPong.Interfaces;

internal interface IGameState
{
    void Ready();
    void Play();
    void Pause();
    void Stop();
    void SetGameState(GameStates mygameState);
    GameEnum.GameStates GetGameState();
}