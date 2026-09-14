using Xunit;
using GameStates = HardPong.GameEnum.GameStates;

namespace HardPong.Tests;

public class GameInputMapperTests
{
    [Theory]
    [InlineData(GameStates.Ready, GameAction.StartMatch)]
    [InlineData(GameStates.Playing, GameAction.Pause)]
    [InlineData(GameStates.Paused, GameAction.Resume)]
    [InlineData(GameStates.Stop, GameAction.PrepareNextRound)]
    public void FromGameplay_WithEnter_MapsActionPerState(GameStates state, GameAction expected)
    {
        var action = HardPong.GameInputMapper.FromGameplay(state, enterPressed: true, escapePressed: false);

        Assert.Equal(expected, action);
    }

    [Theory]
    [InlineData(GameStates.Ready)]
    [InlineData(GameStates.Playing)]
    [InlineData(GameStates.Paused)]
    [InlineData(GameStates.Stop)]
    public void FromGameplay_WithEscape_AlwaysOpensExitMenu(GameStates state)
    {
        var action = HardPong.GameInputMapper.FromGameplay(state, enterPressed: false, escapePressed: true);

        Assert.Equal(HardPong.GameAction.OpenExitMenu, action);
    }

    [Fact]
    public void FromGameplay_WhenEscapeAndEnterTogether_EscapeWins()
    {
        var action = HardPong.GameInputMapper.FromGameplay(GameStates.Playing, enterPressed: true, escapePressed: true);

        Assert.Equal(HardPong.GameAction.OpenExitMenu, action);
    }

    [Theory]
    [InlineData(GameStates.Ready)]
    [InlineData(GameStates.Playing)]
    [InlineData(GameStates.Paused)]
    [InlineData(GameStates.Stop)]
    public void FromGameplay_WithoutKeys_ReturnsNone(GameStates state)
    {
        var action = HardPong.GameInputMapper.FromGameplay(state, enterPressed: false, escapePressed: false);

        Assert.Equal(HardPong.GameAction.None, action);
    }

    [Theory]
    [InlineData(0, HardPong.GameAction.ContinueGame)]
    [InlineData(1, HardPong.GameAction.ReturnToMainMenu)]
    [InlineData(2, HardPong.GameAction.QuitGame)]
    [InlineData(3, HardPong.GameAction.None)]
    public void FromExitMenu_MapsConfirmedSelectionToAction(int option, HardPong.GameAction expected)
    {
        var action = HardPong.GameInputMapper.FromExitMenu(MenuResult.Confirmed(option));

        Assert.Equal(expected, action);
    }

    [Fact]
    public void FromExitMenu_WhenCancelled_ContinuesGame()
    {
        Assert.Equal(GameAction.ContinueGame, GameInputMapper.FromExitMenu(MenuResult.Cancelled));
    }

    [Fact]
    public void FromExitMenu_WithoutAction_DoesNothing()
    {
        Assert.Equal(GameAction.None, GameInputMapper.FromExitMenu(MenuResult.None));
    }
}
