using Xunit;
using GameStates = HardPong.GameEnum.GameStates;

namespace HardPong.Tests;

// Pruebas del comportamiento ACTUAL de la maquina de estados.
// (El paso "transiciones explicitas" cambiara el toggle de Pause: estas pruebas se actualizaran entonces.)
public class GameStateControllerTests
{
    private static GameStateController NewController() => new();

    [Fact]
    public void NewController_StartsInReady()
    {
        var controller = NewController();

        Assert.Equal(GameStates.Ready, controller.GetGameState());
    }

    [Fact]
    public void Play_FromReady_StartsMatch()
    {
        var controller = NewController();

        controller.Play();

        Assert.Equal(GameStates.Playing, controller.GetGameState());
    }

    [Fact]
    public void Play_WhenAlreadyPlaying_IsIgnored()
    {
        var controller = NewController();
        controller.Play();

        controller.Play();

        Assert.Equal(GameStates.Playing, controller.GetGameState());
    }

    [Fact]
    public void Pause_FromPlaying_Pauses()
    {
        var controller = NewController();
        controller.Play();

        controller.Pause();

        Assert.Equal(GameStates.Paused, controller.GetGameState());
    }

    [Fact]
    public void Pause_FromPaused_Resumes()
    {
        var controller = NewController();
        controller.Play();
        controller.Pause();

        controller.Pause();

        Assert.Equal(GameStates.Playing, controller.GetGameState());
    }

    [Fact]
    public void Pause_FromReady_IsIgnored()
    {
        var controller = NewController();

        controller.Pause();

        Assert.Equal(GameStates.Ready, controller.GetGameState());
    }

    [Fact]
    public void Stop_FromPlaying_GoesToStop()
    {
        var controller = NewController();
        controller.Play();

        controller.Stop();

        Assert.Equal(GameStates.Stop, controller.GetGameState());
    }

    [Fact]
    public void Stop_FromStop_BackToReady()
    {
        var controller = NewController();
        controller.Play();
        controller.Stop();

        controller.Stop();

        Assert.Equal(GameStates.Ready, controller.GetGameState());
    }

    [Fact]
    public void Stop_FromReady_IsIgnored()
    {
        var controller = NewController();

        controller.Stop();

        Assert.Equal(GameStates.Ready, controller.GetGameState());
    }

    [Fact]
    public void OpenExitMenu_FromPlaying_RemembersState()
    {
        var controller = NewController();
        controller.Play();

        controller.OpenExitMenu();

        Assert.Equal(GameStates.ExitMenu, controller.GetGameState());

        controller.ResumeFromExitMenu();

        Assert.Equal(GameStates.Playing, controller.GetGameState());
    }

    [Fact]
    public void OpenExitMenu_FromPaused_RemembersState()
    {
        var controller = NewController();
        controller.Play();
        controller.Pause();

        controller.OpenExitMenu();
        controller.ResumeFromExitMenu();

        Assert.Equal(GameStates.Paused, controller.GetGameState());
    }

    [Fact]
    public void OpenExitMenu_Twice_KeepsFirstPreviousState()
    {
        var controller = NewController();
        controller.Play();

        controller.OpenExitMenu();
        controller.OpenExitMenu(); // ignorado: ya estamos en ExitMenu
        controller.ResumeFromExitMenu();

        Assert.Equal(GameStates.Playing, controller.GetGameState());
    }

    [Fact]
    public void ResumeFromExitMenu_OutsideExitMenu_IsIgnored()
    {
        var controller = NewController();
        controller.Play();

        controller.ResumeFromExitMenu();

        Assert.Equal(GameStates.Playing, controller.GetGameState());
    }

    [Fact]
    public void OpenExitMenu_FromReady_RestoresReadyOnResume()
    {
        var controller = NewController();

        controller.OpenExitMenu();
        controller.ResumeFromExitMenu();

        Assert.Equal(GameStates.Ready, controller.GetGameState());
    }
}
