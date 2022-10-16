using App.System.Utils;
using UnityEngine;

namespace App.System.Sound
{
    public class SoundController : MonoSingleton<SoundController>
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip menuMusic;

        bool isSoundEnabled = false;

        [SerializeField]
        float bgmVolume;
        [SerializeField]
        private float sfxvolume;

        public float BgmVolume { get => bgmVolume; }
        public float Sfxvolume { get => sfxvolume; }


        public void UpdateBGM(float volume)
        {
            bgmVolume = volume;
            audioSource.volume = volume;
        }
        public void UpdateSFX(float volume)
        {
            sfxvolume = volume;
        }

        protected override void Dispose()
        {

        }

        private void Awake()
        {
            if (GetInstance() != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        protected override void Init()
        {
            DontDestroyOnLoad(gameObject);

            audioSource.clip = menuMusic;
            audioSource.Play();
        }
    }
}