using Microsoft.Xna.Framework.Audio;

namespace HardPong;

internal class BallSound : ISoundEffect
{
    private readonly SoundEffectInstance _soundEffectInstance;

    public BallSound(SoundEffect soundWall) {
        _soundEffectInstance = soundWall.CreateInstance();
    }
    public void PlaySoundEffect()
    {
        _soundEffectInstance.Play();
    }

    public void StopSoundEffect()
    {
        _soundEffectInstance.Stop();
    }

    public void Dispose()
    {
        _soundEffectInstance.Dispose();
    }
}