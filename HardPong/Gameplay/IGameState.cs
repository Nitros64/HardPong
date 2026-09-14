using static HardPong.GameEnum;

namespace HardPong;

internal interface IGameState
{
    void Ready();
    void Play();
    void Pause();
    void Resume();
    void EndRound();
    void PrepareNextRound();
    void OpenExitMenu();
    void ResumeFromExitMenu();
    GameEnum.GameStates GetGameState();
}