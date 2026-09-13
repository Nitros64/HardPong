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
    [InlineData(0, HardPong.GameAction.ContinueGame)]  // Escape
    [InlineData(1, HardPong.GameAction.ContinueGame)]  // Enter sobre CONTINUE
    [InlineData(2, HardPong.GameAction.ReturnToMainMenu)]
    [InlineData(3, HardPong.GameAction.QuitGame)]
    [InlineData(-1, HardPong.GameAction.None)]         // ninguna seleccion este frame
    public void FromExitMenu_MapsSelectionToAction(int option, HardPong.GameAction expected)
    {
        var action = HardPong.GameInputMapper.FromExitMenu(option);

        Assert.Equal(expected, action);
    }
}
