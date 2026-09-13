using static HardPong.GameEnum;

namespace HardPong;

// Mapeo puro: entrada leida + estado actual -> accion de juego.
// No conoce sprites, pantallas ni audio; solo traduce.
internal static class GameInputMapper
{
    public static GameAction FromGameplay(GameStates state, bool enterPressed, bool escapePressed)
    {
        if (escapePressed)
            return GameAction.OpenExitMenu;

        if (!enterPressed)
            return GameAction.None;

        return state switch
        {
            GameStates.Ready => GameAction.StartMatch,
            GameStates.Playing => GameAction.Pause,
            GameStates.Paused => GameAction.Resume,
            GameStates.Stop => GameAction.PrepareNextRound,
            _ => GameAction.None
        };
    }

    public static GameAction FromExitMenu(int option) => option switch
    {
        0 => GameAction.ContinueGame,   // Escape
        1 => GameAction.ContinueGame,   // Enter sobre CONTINUE
        2 => GameAction.ReturnToMainMenu,
        3 => GameAction.QuitGame,
        _ => GameAction.None
    };
}
