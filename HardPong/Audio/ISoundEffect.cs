using System;

namespace HardPong;
public interface ISoundEffect : IDisposable
{
    void PlaySoundEffect();
    void StopSoundEffect();
}