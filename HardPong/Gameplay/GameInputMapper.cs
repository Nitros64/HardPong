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

    public static GameAction FromExitMenu(MenuResult result) => result switch
    {
        { Kind: MenuResultKind.Cancelled } => GameAction.ContinueGame,
        { Kind: MenuResultKind.Confirmed, SelectedIndex: 0 } => GameAction.ContinueGame,
        { Kind: MenuResultKind.Confirmed, SelectedIndex: 1 } => GameAction.ReturnToMainMenu,
        { Kind: MenuResultKind.Confirmed, SelectedIndex: 2 } => GameAction.QuitGame,
        _ => GameAction.None
    };
}
