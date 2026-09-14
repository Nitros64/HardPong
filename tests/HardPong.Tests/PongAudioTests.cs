using Xunit;

namespace HardPong.Tests;

public class PongAudioTests
{
    [Theory]
    [InlineData(0, "")]
    [InlineData(1, "paddle:play")]
    [InlineData(2, "score:play")]
    [InlineData(3, "paddle:play,score:play")]
    [InlineData(4, "wall:play")]
    [InlineData(5, "paddle:play,wall:play")]
    [InlineData(6, "score:play,wall:play")]
    [InlineData(7, "paddle:play,score:play,wall:play")]
    public void PlayEvents_MapsAllCombinationsInPaddleScoreWallOrder(int events, string expected)
    {
        var calls = new List<string>();
        using var audio = new PongAudio(new RecordingSound("paddle", calls),
            new RecordingSound("wall", calls), new RecordingSound("score", calls));

        audio.PlayEvents((CollisionEvents)events);

        Assert.Equal(expected, string.Join(",", calls));
    }

    [Fact]
    public void StopAll_StopsEverySoundAndKeepsItAvailableForReentry()
    {
        var calls = new List<string>();
        using var audio = new PongAudio(new RecordingSound("paddle", calls),
            new RecordingSound("wall", calls), new RecordingSound("score", calls));

        audio.StopAll();
        audio.PlayPaddle();
        audio.PlayWall();
        audio.PlayScore();

        Assert.Equal(new[]
        {
            "paddle:stop", "wall:stop", "score:stop",
            "paddle:play", "wall:play", "score:play"
        }, calls);
    }

    private sealed class RecordingSound(string name, List<string> calls) : ISoundEffect
    {
        public void PlaySoundEffect() => calls.Add(name + ":play");
        public void StopSoundEffect() => calls.Add(name + ":stop");
        public void Dispose() => calls.Add(name + ":dispose");
    }
}
