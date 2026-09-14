using System;

namespace HardPong;

// Dueño del audio de la partida: la fisica no sabe que el sonido existe.
internal class PongAudio : IDisposable
{
    private readonly ISoundEffect _paddle;
    private readonly ISoundEffect _wall;
    private readonly ISoundEffect _score;

    public PongAudio(ISoundEffect paddle, ISoundEffect wall, ISoundEffect score)
    {
        _paddle = paddle ?? throw new ArgumentNullException(nameof(paddle));
        _wall = wall ?? throw new ArgumentNullException(nameof(wall));
        _score = score ?? throw new ArgumentNullException(nameof(score));
    }

    public void PlayPaddle() => _paddle.PlaySoundEffect();

    public void PlayWall() => _wall.PlaySoundEffect();

    public void PlayScore() => _score.PlaySoundEffect();

    public void StopScore() => _score.StopSoundEffect();

    public void Dispose()
    {
        _paddle.Dispose();
        _wall.Dispose();
        _score.Dispose();
    }
}
