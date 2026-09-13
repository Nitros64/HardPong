using Xunit;
using GameStates = HardPong.GameEnum.GameStates;

namespace HardPong.Tests;

// Pruebas de la maquina de estados con transiciones explicitas.
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
    public void Pause_Twice_DoesNotResume()
    {
        var controller = NewController();
        controller.Play();
        controller.Pause();

        controller.Pause();

        Assert.Equal(GameStates.Paused, controller.GetGameState());
    }

    [Fact]
    public void Resume_FromPaused_Resumes()
    {
        var controller = NewController();
        controller.Play();
        controller.Pause();

        controller.Resume();

        Assert.Equal(GameStates.Playing, controller.GetGameState());
    }

    [Fact]
    public void Resume_FromPlaying_IsIgnored()
    {
        var controller = NewController();
        controller.Play();

        controller.Resume();

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
    public void EndRound_FromPlaying_GoesToStop()
    {
        var controller = NewController();
        controller.Play();

        controller.EndRound();

        Assert.Equal(GameStates.Stop, controller.GetGameState());
    }

    [Fact]
    public void EndRound_FromStop_IsIgnored()
    {
        var controller = NewController();
        controller.Play();
        controller.EndRound();

        controller.EndRound();

        Assert.Equal(GameStates.Stop, controller.GetGameState());
    }

    [Fact]
    public void PrepareNextRound_FromStop_BackToReady()
    {
        var controller = NewController();
        controller.Play();
        controller.EndRound();

        controller.PrepareNextRound();

        Assert.Equal(GameStates.Ready, controller.GetGameState());
    }

    [Fact]
    public void PrepareNextRound_FromPlaying_IsIgnored()
    {
        var controller = NewController();
        controller.Play();

        controller.PrepareNextRound();

        Assert.Equal(GameStates.Playing, controller.GetGameState());
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
