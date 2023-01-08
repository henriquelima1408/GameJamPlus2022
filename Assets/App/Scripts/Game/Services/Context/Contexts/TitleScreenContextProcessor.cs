using System;
using Assets.App.Scripts.System.Utils;
using UnityEngine;
using UnityFx.Async;
using UnityFx.Async.Promises;

namespace App.Game.Services
{
    public class TitleScreenContextProcessor : IContextProcessor
    {
        const string sceneName = "Game";
        const string screenBundle = "screens";
        const string screenPrefabName = "TitleScreen";

        readonly ISceneLoaderService sceneLoaderService;
        readonly IBundleService bundleService;


        public TitleScreenContextProcessor(IAsyncOperation<ISceneLoaderService> sceneLoaderService, IAsyncOperation<IBundleService> bundleService)
        {
            this.sceneLoaderService = sceneLoaderService.Result;
            this.bundleService = bundleService.Result;
        }

        public void Apply()
        {
            var scenePromise = sceneLoaderService.LoadSceneAsync(sceneName);
            scenePromise.Then((scene) =>
            {
                var viewRoot = scene.GetComponentInRootObjects<ViewRoot>(false);
                var prefabPromise = bundleService.LoadAsset<GameObject>(screenBundle, screenPrefabName);

                prefabPromise.Then((prefab) =>
                {
                    MonoBehaviour.Instantiate(prefab, viewRoot.ScreenRoot);
                }).Catch((e) => Debug.LogException(e));

            }).Catch((e) => Debug.LogException(e));

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
