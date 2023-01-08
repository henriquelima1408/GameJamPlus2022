using System;
using UnityEngine;
using UnityFx.Async;
using System.Collections;
using System.Collections.Generic;
using UnityFx.Async.Promises;


namespace App.Game.Services
{
    public class SoundService : ISoundService
    {
        float bgmVolume = 1;
        float sfxvolume = 1;

        public float BgmVolume { get => bgmVolume; }
        public float Sfxvolume { get => sfxvolume; }

        public bool IsInitialized => true;

        readonly int bgmPoolCount = 2;
        readonly int sfxPoolCount = 5;

        readonly Queue<AudioSource> bgmSourcePool;
        readonly Queue<AudioSource> sfxSourcePool;
        readonly HashSet<AudioSource> currentlyPlayingAudioSources;

        AudioSource currentBGMSource;

        //Dependencies
        ICoroutineService coroutineService;

        public SoundService(IAsyncOperation<ICoroutineService> asyncCoroutineHelper)
        {
            bgmSourcePool = new Queue<AudioSource>(bgmPoolCount);
            sfxSourcePool = new Queue<AudioSource>(sfxPoolCount);
            currentlyPlayingAudioSources = new HashSet<AudioSource>();
            asyncCoroutineHelper.Then<ICoroutineService>((c) => { this.coroutineService = c; });

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

        void ResetAudioSource(AudioSource audioSource)
        {
            if (audioSource == null)
            {
                Debug.LogError("Audio Source is Empty");
                return;
            }

            currentlyPlayingAudioSources.Remove(audioSource);
            audioSource.Stop();
            audioSource.clip = null;
        }

        public void PlayBGM(AudioClip audioClip, SoundDetails soundDetails)
        {
            if (soundDetails.transitionEnabled)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (currentBGMSource != null)
                    ResetAudioSource(currentBGMSource);

                currentBGMSource = bgmSourcePool.Dequeue();
                PlaySound(currentBGMSource, audioClip, soundDetails).
                    Then((audioSource) =>
                    {
                        ResetAudioSource(audioSource);
                        bgmSourcePool.Enqueue(audioSource);
                    }).Catch((e) => Debug.LogException(e));
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
            }).Catch((e) => Debug.LogException(e));
        }

        public void PauseAllSFX(bool state)
        {

            foreach (var audioSource in currentlyPlayingAudioSources)
            {
                if (audioSource != currentBGMSource)
                {
                    if (state)
                    {
                        audioSource.Pause();

                    }
                    else
                    {
                        audioSource.UnPause();
                    }
                }
            }

        }

        public void StopAllSFX()
        {
            foreach (var audioSource in currentlyPlayingAudioSources)
            {
                if (audioSource != currentBGMSource) audioSource?.Pause();
            }
        }

        IAsyncOperation<AudioSource> PlaySound(AudioSource audioSource, AudioClip audioClip, SoundDetails transitionDetails)
        {
            currentlyPlayingAudioSources.Add(audioSource);

            audioSource.clip = audioClip;
            audioSource.Play();

            var asyncCompletionSource = new AsyncCompletionSource<AudioSource>();

            coroutineService.AddCoroutine(WaitAudioSourceFinishPlay(asyncCompletionSource, audioSource)).AsyncOperation.
                Then((coroutineID) => coroutineService.RemoveCoroutine(coroutineID)).Catch((e) => Debug.LogException(e));

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

                yield return null;
            }

            asyncOperation.SetResult(audioSource);
        }

        public void Dispose()
        {

        }
    }
}