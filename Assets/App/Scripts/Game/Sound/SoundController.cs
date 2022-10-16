using App.System.Utils;
using UnityEngine;

namespace App.System.Sound
{
    public class SoundController : MonoSingleton<SoundController>
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip menuMusic;

        bool isSoundEnabled = false;

        public bool IsSoundEnabled { get => isSoundEnabled; }

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