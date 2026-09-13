using System;

namespace HardPong.Interfaces;
public interface ISoundEffect : IDisposable
{
    void PlaySoundEffect();
    void StopSoundEffect();
}