using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityFx.Async;

namespace App.Game.Services
{
    public interface ISceneLoaderService : IService
    {
        IEnumerable<string> LoadedSceneNames { get; }
        Scene LoadScene(string sceneName, bool isAdditive = false);
        IAsyncOperation<Scene> LoadSceneAsync(string sceneName, bool isAdditive = false);
        void UnloadScene(string sceneName);
        IAsyncOperation UnloadSceneAsync(string sceneName);
    }
}
