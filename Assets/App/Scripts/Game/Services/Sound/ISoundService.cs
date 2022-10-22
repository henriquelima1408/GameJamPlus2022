using UnityEngine;

namespace App.Game.Services.SoudServiceMock
{
    public interface ISoundService : IService
    {
        void PlayBGM(AudioClip audioClip, SoundDetails soundDetails);
        void PauseBGM(bool state);
        void StopBGM();
        void PlaySFX(AudioClip audioClip);
        void PauseAllSFX(bool state);
        void StopAllSFX();
    }
}
namespace App.Game.Services.SoudServiceMock
{

    public struct SoundDetails
    {
        readonly bool isLoop;
        readonly bool transition;
        readonly float fadeOutTime;
        readonly float fadeInTime;

        public SoundDetails(bool isLoop, bool transition, float fadeOutTime, float fadeInTime)
        {
            this.isLoop = isLoop;
            this.transition = transition;
            this.fadeOutTime = fadeOutTime;
            this.fadeInTime = fadeInTime;
        }

        public bool transitionEnabled => transition;

        public float FadeOutTime => fadeOutTime;

        public float FadeInTime => fadeInTime;

        public bool IsLoop => isLoop;
    }

}
