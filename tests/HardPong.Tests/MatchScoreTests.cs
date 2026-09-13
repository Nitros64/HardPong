using Xunit;

namespace HardPong.Tests;

public class MatchScoreTests
{
    private static MatchScore NewMatch() => new();

    [Fact]
    public void NewMatch_HasNoPointsAndNoWinner()
    {
        var score = NewMatch();

        Assert.Equal(0, score.Player1);
        Assert.Equal(0, score.Player2);
        Assert.Equal(PlayerId.None, score.LastPointWinner);
        Assert.Equal(PlayerId.None, score.MatchWinner);
    }

    [Fact]
    public void AwardPoint_ToPlayer1_IncrementsAndRemembers()
    {
        var score = NewMatch();

        score.AwardPoint(PlayerId.Player1);

        Assert.Equal(1, score.Player1);
        Assert.Equal(0, score.Player2);
        Assert.Equal(PlayerId.Player1, score.LastPointWinner);
    }

    [Fact]
    public void AwardPoint_ToPlayer2_IncrementsAndRemembers()
    {
        var score = NewMatch();

        score.AwardPoint(PlayerId.Player2);

        Assert.Equal(1, score.Player2);
        Assert.Equal(PlayerId.Player2, score.LastPointWinner);
    }

    [Fact]
    public void AwardPoint_ToNone_IsIgnored()
    {
        var score = NewMatch();

        score.AwardPoint(PlayerId.None);

        Assert.Equal(0, score.Player1);
        Assert.Equal(0, score.Player2);
        Assert.Equal(PlayerId.None, score.LastPointWinner);
    }

    [Fact]
    public void AtTenPoints_Player1WinsTheMatch()
    {
        var score = NewMatch();
        for (var i = 0; i < 9; i++)
            score.AwardPoint(PlayerId.Player1);
        Assert.Equal(PlayerId.None, score.MatchWinner); // 9 puntos aun no bastan

        score.AwardPoint(PlayerId.Player1);

        Assert.Equal(10, score.Player1);
        Assert.Equal(PlayerId.Player1, score.MatchWinner);
    }

    [Fact]
    public void AtTenPoints_Player2WinsTheMatch()
    {
        var score = NewMatch();
        for (var i = 0; i < 10; i++)
            score.AwardPoint(PlayerId.Player2);

        Assert.Equal(PlayerId.Player2, score.MatchWinner);
    }

    [Fact]
    public void AfterMatchWon_FurtherPointsAreIgnored()
    {
        var score = NewMatch();
        for (var i = 0; i < 10; i++)
            score.AwardPoint(PlayerId.Player1);

        score.AwardPoint(PlayerId.Player2);

        Assert.Equal(0, score.Player2);
        Assert.Equal(PlayerId.Player1, score.LastPointWinner);
        Assert.Equal(PlayerId.Player1, score.MatchWinner);
    }

    [Fact]
    public void StartNextRound_ClearsLastPointWinner_ButKeepsScoreAndMatch()
    {
        var score = NewMatch();
        score.AwardPoint(PlayerId.Player1);

        score.StartNextRound();

        Assert.Equal(PlayerId.None, score.LastPointWinner);
        Assert.Equal(1, score.Player1);
        Assert.Equal(PlayerId.None, score.MatchWinner);
    }

    [Fact]
    public void StartNextRound_AfterMatchWon_KeepsMatchWinner()
    {
        var score = NewMatch();
        for (var i = 0; i < 10; i++)
            score.AwardPoint(PlayerId.Player1);

        score.StartNextRound();

        Assert.Equal(PlayerId.Player1, score.MatchWinner);
    }

    [Fact]
    public void Reset_ClearsEverything()
    {
        var score = NewMatch();
        for (var i = 0; i < 10; i++)
            score.AwardPoint(PlayerId.Player2);

        score.Reset();

        Assert.Equal(0, score.Player1);
        Assert.Equal(0, score.Player2);
        Assert.Equal(PlayerId.None, score.LastPointWinner);
        Assert.Equal(PlayerId.None, score.MatchWinner);
    }
}
