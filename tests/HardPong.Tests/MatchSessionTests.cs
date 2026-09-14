using HardPong.Dependencies;
using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;
using Xunit;
using GameStates = HardPong.GameEnum.GameStates;

namespace HardPong.Tests;

// Simulacion de partida completa sin ventana, texturas ni sonido.
public class MatchSessionTests
{
    private sealed class SilentSound : ISoundEffect
    {
        public void PlaySoundEffect() { }
        public void StopSoundEffect() { }
        public void Dispose() { }
    }

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
        var audio = new PongAudio(new SilentSound(), new SilentSound(), new SilentSound());
        return new MatchSession(ball, player1, player2, audio, Court);
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
    public void PauseAndResume_SwitchWithoutTouchingTheBall()
    {
        var match = NewMatch();
        match.Execute(HardPong.GameAction.StartMatch);
        var ballBefore = match.Ball.Position;

        match.Execute(HardPong.GameAction.Pause);
        Assert.Equal(GameStates.Paused, match.State);

        match.Execute(HardPong.GameAction.Resume);
        Assert.Equal(GameStates.Playing, match.State);
        Assert.Equal(ballBefore, match.Ball.Position);
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

        // El flujo real tras la victoria anade RestartMatch() en la pantalla:
        match.ResetMatch();

        Assert.Equal(GameStates.Ready, match.State);
        Assert.Equal(0, match.Score.Player1);
        Assert.Equal(0, match.Score.Player2);
        Assert.Equal(PlayerId.None, match.Score.MatchWinner);
        Assert.False(match.IsMatchOver);
    }
}
