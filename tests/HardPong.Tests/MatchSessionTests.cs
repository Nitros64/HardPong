using Microsoft.Xna.Framework;
using Xunit;
using GameStates = HardPong.GameEnum.GameStates;

namespace HardPong.Tests;

// Simulacion de partida completa sin ventana, texturas ni sonido.
public class MatchSessionTests
{
    private sealed class FixedController(float axis) : IPaddleController
    {
        public float ReadMovementAxis() => axis;
    }

    private static readonly Rectangle Court = new(0, 0, 800, 480);

    private static MatchSession NewMatch()
    {
        var ball = new Ball(Court);
        var player1 = new Paddle(new Vector2(10, Court.Height / 2 - Paddle.BrickHeight / 2), new FixedController(0f));
        var player2 = new Paddle(new Vector2(Court.Width - 25, Court.Height / 2 - Paddle.BrickHeight / 2), new FixedController(0f));
        return new MatchSession(ball, player1, player2, Court);
    }

    [Fact]
    public void SimulateFrame_WithoutCollision_ReturnsNoEvents()
    {
        var match = NewMatch();
        match.Execute(GameAction.StartMatch);

        Assert.Equal(CollisionEvents.None, match.SimulateFrame(Court));
        Assert.Equal(GameStates.Playing, match.State);
    }

    [Fact]
    public void SimulateFrame_WallHit_ReportsImpactWithoutScoring()
    {
        var match = NewMatch();
        match.Execute(GameAction.StartMatch);
        match.Ball.SetPosition(400, Court.Height - Ball.BallHeight);

        Assert.Equal(CollisionEvents.WallHit, match.SimulateFrame(Court));
        Assert.True(match.Ball.Direction.Y < 0);
        Assert.Equal(0, match.Score.Player1);
        Assert.Equal(0, match.Score.Player2);
        Assert.Equal(GameStates.Playing, match.State);
        Assert.Equal(CollisionEvents.None, match.SimulateFrame(Court));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SimulateFrame_PaddleHit_ReportsImpactAndReversesHorizontalTravel(bool left)
    {
        var match = NewMatch();
        match.Execute(GameAction.StartMatch);
        if (left)
            match.Ball.InvertDirectionHorizontal();
        float x = left ? match.Player1.Position.X + Paddle.BrickWidth
            : match.Player2.Position.X - Ball.BallWidth;
        float y = match.Player1.Position.Y + Paddle.BrickHeight / 2 - Ball.BallHeight / 2 - Ball.BallSpeedY;
        match.Ball.SetPosition(x, y);

        Assert.Equal(CollisionEvents.PaddleHit, match.SimulateFrame(Court));
        Assert.Equal(left, match.Ball.Direction.X > 0);
        Assert.Equal(0, match.Score.Player1);
        Assert.Equal(0, match.Score.Player2);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void SimulateFrame_Point_ReportsSimultaneousWallHitAndDoesNotRepeat(bool left, bool corner)
    {
        var match = NewMatch();
        match.Execute(GameAction.StartMatch);
        if (left)
            match.Ball.InvertDirectionHorizontal();
        match.Ball.SetPosition(left ? 0 : Court.Width - Ball.BallWidth,
            corner ? Court.Height - Ball.BallHeight : 100);
        var expected = CollisionEvents.PointScored;
        if (corner)
            expected |= CollisionEvents.WallHit;

        Assert.Equal(expected, match.SimulateFrame(Court));
        Assert.Equal(left ? PlayerId.Player2 : PlayerId.Player1, match.Score.LastPointWinner);
        Assert.Equal(left ? 0 : 1, match.Score.Player1);
        Assert.Equal(left ? 1 : 0, match.Score.Player2);
        Assert.Equal(GameStates.Stop, match.State);
        Assert.Equal(CollisionEvents.None, match.SimulateFrame(Court));
    }

    [Fact]
    public void NewMatch_StartsReadyWithCleanScore()
    {
        var match = NewMatch();

        Assert.Equal(GameStates.Ready, match.State);
        Assert.Equal(0, match.Score.Player1);
        Assert.Equal(0, match.Score.Player2);
        Assert.False(match.IsMatchOver);
    }

    [Fact]
    public void StartMatch_MovesIntoPlaying()
    {
        var match = NewMatch();

        match.Execute(HardPong.GameAction.StartMatch);

        Assert.Equal(GameStates.Playing, match.State);
    }

    [Fact]
    public void PauseAndResume_StopsSimulationUntilResumed()
    {
        var match = NewMatch();
        match.Execute(HardPong.GameAction.StartMatch);
        var ballBefore = match.Ball.Position;

        match.Execute(HardPong.GameAction.Pause);
        Assert.Equal(GameStates.Paused, match.State);
        match.SimulateFrame(Court);
        Assert.Equal(ballBefore, match.Ball.Position);

        match.Execute(HardPong.GameAction.Resume);
        Assert.Equal(GameStates.Playing, match.State);
        match.SimulateFrame(Court);
        Assert.Equal(ballBefore + new Vector2(Ball.BallSpeedX, Ball.BallSpeedY), match.Ball.Position);
    }

    [Fact]
    public void BallReachingRightBoundary_ScoresForPlayer1AndEndsRound()
    {
        var match = NewMatch();
        match.Execute(HardPong.GameAction.StartMatch);

        for (var frame = 0; frame < 500 && match.State == GameStates.Playing; frame++)
            match.SimulateFrame(Court);

        Assert.Equal(GameStates.Stop, match.State);
        Assert.Equal(1, match.Score.Player1);
        Assert.Equal(PlayerId.Player1, match.Score.LastPointWinner);
    }

    [Fact]
    public void PrepareNextRound_AfterAPoint_RecentersAndCleansThePoint()
    {
        var match = NewMatch();
        match.Execute(HardPong.GameAction.StartMatch);
        for (var frame = 0; frame < 500 && match.State == GameStates.Playing; frame++)
            match.SimulateFrame(Court);

        var result = match.PrepareNextRound();

        Assert.Equal(MatchEndResult.NextRound, result);
        Assert.Equal(GameStates.Ready, match.State);
        Assert.Equal(1, match.Score.Player1);
        Assert.Equal(PlayerId.None, match.Score.LastPointWinner);
        Assert.False(match.IsMatchOver);
        Assert.Equal(new Vector2(800 / 2 - Ball.BallWidth / 2, 480 / 2 - Ball.BallHeight / 2), match.Ball.Position);
    }

    // Nota: el saque conserva el signo de la velocidad anterior, por lo que
    // los saques alternan de lado ronda a ronda (comportamiento historico).
    [Fact]
    public void RepeatedRounds_EventuallySomeoneWinsTheMatch()
    {
        var match = NewMatch();
        var result = MatchEndResult.NextRound;
        var rounds = 0;

        while (result != MatchEndResult.MatchWon && rounds < 30)
        {
            match.Execute(HardPong.GameAction.StartMatch);
            for (var frame = 0; frame < 500 && match.State == GameStates.Playing; frame++)
                match.SimulateFrame(Court);

            var winner = match.Score.MatchWinner;
            var winnerPoints = winner == PlayerId.Player1
                ? match.Score.Player1
                : match.Score.Player2;

            result = match.PrepareNextRound();
            rounds++;

            if (result == MatchEndResult.MatchWon)
                Assert.Equal(10, winnerPoints);
            else
            {
                Assert.Equal(MatchEndResult.NextRound, result);
                Assert.Equal(PlayerId.None, winner);
            }
        }

        Assert.Equal(MatchEndResult.MatchWon, result);
    }

    [Fact]
    public void AfterMatchWon_PrepareNextRound_ResetsAndReportsTheWin()
    {
        var match = NewMatch();
        var result = MatchEndResult.NextRound;
        var rounds = 0;
        while (result != MatchEndResult.MatchWon && rounds < 30)
        {
            match.Execute(HardPong.GameAction.StartMatch);
            for (var frame = 0; frame < 500 && match.State == GameStates.Playing; frame++)
                match.SimulateFrame(Court);
            result = match.PrepareNextRound();
            rounds++;
        }
        Assert.Equal(MatchEndResult.MatchWon, result);

        Assert.Equal(GameStates.Ready, match.State);
        Assert.Equal(0, match.Score.Player1);
        Assert.Equal(0, match.Score.Player2);
        Assert.Equal(PlayerId.None, match.Score.LastPointWinner);
        Assert.Equal(PlayerId.None, match.Score.MatchWinner);
        Assert.False(match.IsMatchOver);
        Assert.Equal(new Vector2(Court.Width / 2 - Ball.BallWidth / 2,
            Court.Height / 2 - Ball.BallHeight / 2), match.Ball.Position);
        Assert.Equal(new Vector2(10, Court.Height / 2 - Paddle.BrickHeight / 2), match.Player1.Position);
        Assert.Equal(new Vector2(Court.Width - 25, Court.Height / 2 - Paddle.BrickHeight / 2), match.Player2.Position);
    }
}
