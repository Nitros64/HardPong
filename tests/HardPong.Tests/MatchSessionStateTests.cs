using Microsoft.Xna.Framework;
using Xunit;
using GameStates = HardPong.GameEnum.GameStates;

namespace HardPong.Tests;

public class MatchSessionStateTests
{
    private static readonly Rectangle Court = new(0, 0, 800, 480);

    [Theory]
    [InlineData(GameStates.Ready)]
    [InlineData(GameStates.Paused)]
    [InlineData(GameStates.Stop)]
    [InlineData(GameStates.ExitMenu)]
    public void SimulateFrame_OutsidePlaying_HasNoEffects(GameStates state)
    {
        var scenario = new Scenario(state);
        // Una simulacion accidental moveria las palas y provocaria un punto
        // y un rebote en el muro, ademas de reproducir sonidos.
        scenario.Match.Ball.SetPosition(Court.Width - Ball.BallWidth, Court.Height - Ball.BallHeight);
        var before = scenario.Capture();

        for (int frame = 0; frame < 3; frame++)
            scenario.Match.SimulateFrame(Court);

        Assert.Equal(before, scenario.Capture());
    }

    [Theory]
    [InlineData(GameStates.Ready)]
    [InlineData(GameStates.Playing)]
    [InlineData(GameStates.Paused)]
    [InlineData(GameStates.ExitMenu)]
    public void PrepareNextRound_OutsideStop_IsIgnoredWithoutEffects(GameStates state)
    {
        var scenario = new Scenario(state);
        // Posiciones distintas al saque para detectar un reinicio accidental.
        scenario.Match.Ball.SetPosition(300, 100);
        scenario.Match.Player1.SetPosition(10, 100);
        scenario.Match.Player2.SetPosition(Court.Width - 25, 300);
        var before = scenario.Capture();

        var result = scenario.Match.PrepareNextRound();

        Assert.Equal(MatchEndResult.Ignored, result);
        Assert.Equal(before, scenario.Capture());
    }

    [Fact]
    public void PrepareNextRound_FromExitMenu_PreservesPointUntilMenuIsClosed()
    {
        var scenario = new Scenario(GameStates.Stop);
        scenario.Match.Execute(GameAction.OpenExitMenu);
        var before = scenario.Capture();

        Assert.Equal(MatchEndResult.Ignored, scenario.Match.PrepareNextRound());
        Assert.Equal(before, scenario.Capture());

        scenario.Match.Execute(GameAction.ContinueGame);
        Assert.Equal(GameStates.Stop, scenario.Match.State);
        Assert.Equal(PlayerId.Player1, scenario.Match.Score.LastPointWinner);
        Assert.Equal(MatchEndResult.NextRound, scenario.Match.PrepareNextRound());
        Assert.Equal(GameStates.Ready, scenario.Match.State);
        Assert.Equal(1, scenario.Match.Score.Player1);
        Assert.Equal(PlayerId.None, scenario.Match.Score.LastPointWinner);
        Assert.Equal("score:stop", scenario.AudioEvents[^1]);
    }

    [Fact]
    public void PrepareNextRound_Twice_AppliesEffectsOnlyOnce()
    {
        var scenario = new Scenario(GameStates.Stop);
        int soundCallsBefore = scenario.AudioEvents.Count;

        Assert.Equal(MatchEndResult.NextRound, scenario.Match.PrepareNextRound());
        Assert.Equal(soundCallsBefore + 1, scenario.AudioEvents.Count);
        Assert.Equal("score:stop", scenario.AudioEvents[^1]);
        var afterFirstCall = scenario.Capture();

        Assert.Equal(MatchEndResult.Ignored, scenario.Match.PrepareNextRound());
        Assert.Equal(afterFirstCall, scenario.Capture());
    }

    private sealed class Scenario
    {
        private readonly CountingController _controller1 = new(1f);
        private readonly CountingController _controller2 = new(-1f);

        public List<string> AudioEvents { get; } = [];
        public MatchSession Match { get; }

        public Scenario(GameStates state)
        {
            var audio = new PongAudio(
                new RecordingSound("paddle", AudioEvents),
                new RecordingSound("wall", AudioEvents),
                new RecordingSound("score", AudioEvents));
            Match = new MatchSession(new Ball(Court),
                new Paddle(Vector2.Zero, _controller1),
                new Paddle(Vector2.Zero, _controller2), audio, Court);

            if (state != GameStates.Ready)
            {
                Match.Execute(GameAction.StartMatch);
                if (state == GameStates.Stop)
                    Match.Ball.SetPosition(Court.Width - Ball.BallWidth, 100);
                Match.SimulateFrame(Court);

                if (state == GameStates.Paused)
                    Match.Execute(GameAction.Pause);
                else if (state == GameStates.ExitMenu)
                    Match.Execute(GameAction.OpenExitMenu);
            }

            Assert.Equal(state, Match.State);
        }

        public Snapshot Capture() => new(Match.State, Match.Ball.Position, Match.Ball.Direction,
            Match.Player1.Position, Match.Player2.Position, Match.Score.Player1, Match.Score.Player2,
            Match.Score.LastPointWinner, Match.Score.MatchWinner,
            _controller1.ReadCount, _controller2.ReadCount, AudioEvents.Count);
    }

    private readonly record struct Snapshot(GameStates State, Vector2 BallPosition, Vector2 BallVelocity,
        Vector2 Player1Position, Vector2 Player2Position, int Score1, int Score2,
        PlayerId LastPointWinner, PlayerId MatchWinner, int Controller1Reads, int Controller2Reads,
        int AudioCalls);

    private sealed class CountingController(float axis) : IPaddleController
    {
        public int ReadCount { get; private set; }

        public float ReadMovementAxis()
        {
            ReadCount++;
            return axis;
        }
    }

    private sealed class RecordingSound(string name, List<string> events) : ISoundEffect
    {
        public void PlaySoundEffect() => events.Add(name + ":play");
        public void StopSoundEffect() => events.Add(name + ":stop");
        public void Dispose() => events.Add(name + ":dispose");
    }
}
