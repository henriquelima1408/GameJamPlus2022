using System;
using App.System.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFx.Async;
using UnityFx.Async.Promises;

namespace App.Game.Services
{
    public class SoundService : IService
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

        float bgmVolume = 1;
        float sfxvolume = 1;

        public float BgmVolume { get => bgmVolume; }
        public float Sfxvolume { get => sfxvolume; }

        public bool IsInitialized => true;

        readonly int bgmPoolCount = 2;
        readonly int sfxPoolCount = 5;

        readonly Queue<AudioSource> bgmSourcePool;
        readonly Queue<AudioSource> sfxSourcePool;

        AudioSource currentBGMSource;
        //Dependencies
        CoroutineHelper coroutineHelper;

        public SoundService(IAsyncOperation<CoroutineHelper> asyncCoroutineHelper)
        {
            bgmSourcePool = new Queue<AudioSource>(bgmPoolCount);
            sfxSourcePool = new Queue<AudioSource>(sfxPoolCount);
            asyncCoroutineHelper.Then<CoroutineHelper>((c) => { this.coroutineHelper = c; });
        }

        void ResetAudioSource(AudioSource audioSource)
        {
            if (audioSource == null)
            {
                Debug.LogError("Audio Source is Empty");
                return;
            }

            audioSource.Stop();
            audioSource.clip = null;
        }

        public void PlayBGM(AudioClip audioClip, SoundDetails soundDetails)
        {
            if (soundDetails.transitionEnabled)
            {

            }
            else
            {
                currentBGMSource = bgmSourcePool.Dequeue();
                PlaySound(currentBGMSource, audioClip, soundDetails).
                    Then((audioSource) =>
                    {
                        ResetAudioSource(audioSource);
                        bgmSourcePool.Enqueue(audioSource);
                    });
            }

        }

        public void PauseBGM(bool state)
        {

            if (state)
                currentBGMSource?.Pause();
            else
                currentBGMSource?.UnPause();
        }

        public void StopBGM()
        {
            currentBGMSource?.Stop();
        }

        public void PlaySFX(AudioClip audioClip)
        {
            PlaySound(sfxSourcePool.Dequeue(), audioClip, new SoundDetails()).Then((audioSource) =>
            {
                ResetAudioSource(audioSource);
                sfxSourcePool.Enqueue(audioSource);
            });
        }

        IAsyncOperation<AudioSource> PlaySound(AudioSource audioSource, AudioClip audioClip, SoundDetails transitionDetails)
        {
            audioSource.clip = audioClip;
            audioSource.Play();

            var asyncCompletionSource = new AsyncCompletionSource<AudioSource>();

            coroutineHelper.AddCoroutine(WaitAudioSourceFinishPlay(asyncCompletionSource, audioSource)).
                Then((coroutineID) => coroutineHelper.RemoveCoroutine(coroutineID));

            return asyncCompletionSource;
        }

        IEnumerator WaitAudioSourceFinishPlay(IAsyncCompletionSource<AudioSource> asyncOperation, AudioSource audioSource)
        {
            var clip = audioSource.clip;

            float previousSampleRate = 0f;
            float currentsampleRate = 0f;

            float sampleRate = 1f / clip.frequency;


            while (audioSource.loop || audioSource.isPlaying || previousSampleRate <= currentsampleRate)
            {
                previousSampleRate = currentsampleRate;
                currentsampleRate = audioSource.timeSamples * sampleRate;

                Debug.Log(currentsampleRate);
                yield return null;
            }

            asyncOperation.SetResult(audioSource);
        }

        public void Dispose()
        {

        }

        public void Init()
        {


            var root = new GameObject("SoundRoot").transform;
            var bgmRoot = new GameObject("BGMRoot").transform;
            var sfxRoot = new GameObject("SFXRoot").transform;

            bgmRoot.parent = root;
            sfxRoot.parent = root;

            for (int i = 0; i < bgmPoolCount; i++)
            {
                var g = new GameObject($"_BGMSource_{i}");
                var audioSource = g.AddComponent<AudioSource>();
                g.transform.parent = bgmRoot.transform;

                bgmSourcePool.Enqueue(audioSource);
            }

            for (int i = 0; i < sfxPoolCount; i++)
            {
                var g = new GameObject($"_SFXSource_{i}");
                var audioSource = g.AddComponent<AudioSource>();
                g.transform.parent = sfxRoot;

                sfxSourcePool.Enqueue(audioSource);
            }

            MonoBehaviour.DontDestroyOnLoad(root.gameObject);
        }
    }
}