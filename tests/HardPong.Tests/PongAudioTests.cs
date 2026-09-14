using Xunit;

namespace HardPong.Tests;

public class PongAudioTests
{
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
