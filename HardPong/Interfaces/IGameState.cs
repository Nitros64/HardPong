using static HardPong.GameEnum;

namespace HardPong.Interfaces;

internal interface IGameState
{
    void Ready();
    void Play();
    void Pause();
    void Stop();
    void OpenExitMenu();
    void ResumeFromExitMenu();
    GameEnum.GameStates GetGameState();
}