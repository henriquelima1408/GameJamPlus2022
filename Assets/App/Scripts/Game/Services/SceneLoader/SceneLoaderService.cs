using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityFx.Async;

namespace App.Game.Services
{
    public class SceneLoaderService : ISceneLoaderService
    {
        readonly HashSet<string> loadedSceneNames = new HashSet<string>();

        public bool IsInitialized => true;

        public IEnumerable<string> LoadedSceneNames => loadedSceneNames;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Scene LoadScene(string sceneName, bool isAdditive)
        {
            if (loadedSceneNames.Contains(sceneName))
            {
                throw new Exception("Trying to loaded an already loaded scene");
            }

            loadedSceneNames.Add(sceneName);
            SceneManager.LoadScene(sceneName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            return SceneManager.GetSceneByName(sceneName);
        }

        public IAsyncOperation<Scene> LoadSceneAsync(string sceneName, bool isAdditive)
        {
            var asyncCompletionSource = new AsyncCompletionSource<Scene>();
            if (loadedSceneNames.Contains(sceneName))
            {
                throw new Exception("Trying to loaded an already loaded scene");                
            }

            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);

            asyncOperation.completed += (operation) =>
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                asyncCompletionSource.SetResult(scene);
            };

            return asyncCompletionSource;
        }


        public void UnloadScene(string sceneName)
        {
            if (!loadedSceneNames.Contains(sceneName))
            {
                Debug.LogError("Trying to unload a scene that isn't loaded");
                return;
            }

            SceneManager.UnloadScene(sceneName);
        }

        public IAsyncOperation UnloadSceneAsync(string sceneName)
        {
            var asyncCompletionSource = new AsyncCompletionSource();
            if (!loadedSceneNames.Contains(sceneName))
            {
                Debug.LogError("Trying to unload a scene that isn't loaded");
                asyncCompletionSource.SetCompleted();
                return asyncCompletionSource;
            }

            var asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            asyncOperation.completed += (operation) =>
            {

                asyncCompletionSource.SetCompleted();
            };

            return asyncCompletionSource;
        }
    }
}
