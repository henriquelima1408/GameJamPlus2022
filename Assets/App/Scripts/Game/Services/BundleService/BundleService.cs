using System;
using System.IO;
using UnityEngine;
using UnityFx.Async;
using Newtonsoft.Json;
using App.System.Utils;
using UnityFx.Async.Promises;
using System.Collections.Generic;

namespace App.Game.Services
{
    public class BundleService : IBundleService
    {
        const string fileName = "BundleManifest.txt";
        const string fileFolderName = "BundleData";

        readonly string bundleManifestPath;

        BundleManifest bundleManifest;
        Dictionary<string, BundleData> bundles = new Dictionary<string, BundleData>();
        public bool IsInitialized => true;

        public BundleService()
        {

            bundleManifestPath = Path.Combine(Application.streamingAssetsPath, fileFolderName, $"{fileName}");

            Debug.Assert(File.Exists(bundleManifestPath), $"Bundle manifest doesnt exist in path: {bundleManifestPath}");


            using (StreamReader streamReader = new StreamReader(bundleManifestPath))
            {
                var manifestJson = streamReader.ReadToEnd();
                bundleManifest = JsonConvert.DeserializeObject<BundleManifest>(manifestJson);
            }

            foreach (var bundleMetadata in bundleManifest.BundleMetadatas)
            {
                bundles.Add(bundleMetadata.BundleName, new BundleData(bundleMetadata));
            }
        }

        public IAsyncOperation<T> LoadAsset<T>(string bundleName, string assetName) where T : UnityEngine.Object
        {
            var resultOperation = new AsyncCompletionSource<T>();
            LoadBundle(bundleName).
                Then(() =>
                {
                    T asset = null;

                    if (CompilationUtils.IsEditor)
                    {
                        asset = System.Bundles.Editor.BundleAssetProvider.GetAsset<T>(bundleName, assetName);
                    }
                    else
                    {
                        asset = bundles[bundleName].AssetBundle.LoadAsset<T>(assetName);
                    }

                    resultOperation?.SetResult(asset);
                });

            return resultOperation;
        }

        public IAsyncOperation LoadBundle(string bundleName)
        {
            var operation = new AsyncCompletionSource();
            var bundle = bundles[bundleName];

            if (!bundle.IsLoaded)
            {
                Debug.Assert(IsBundleReady(bundleName), $"Bundle {bundleName} isnt ready");
                bundle.LoadBundle();
            }

            operation?.SetCompleted();
            return operation;
        }

        public bool IsBundleReady(string bundleName)
        {
            return bundles[bundleName].IsReady;
        }

        public string[] GetAssetNames(string bundleName)
        {
            return bundles[bundleName].GetAssetNames();
        }

        public void Dispose()
        {
            foreach (var bundleName in bundles.Keys)
            {
                bundles[bundleName].UnloadBundle();
            }

            bundles = null;
            bundleManifest = null;
        }


        [Serializable]
        class BundleData
        {
            readonly string bundleName;
            readonly string bundlePath = "";
            readonly bool isRemote;
            readonly BundleMetadata bundleMetadata;

            AssetBundle assetBundle;

            public string BundleName { get => bundleName; }
            public bool IsRemote { get => isRemote; }
            public bool IsLoaded { get => CompilationUtils.IsEditor || assetBundle != null; }
            public bool IsReady { get => CompilationUtils.IsEditor || !IsRemote; }
            public AssetBundle AssetBundle { get => assetBundle; }

            public BundleData(BundleMetadata bundleMetadata)
            {
                this.bundleMetadata = bundleMetadata;
                this.bundleName = bundleMetadata.BundleName;
                this.isRemote = bundleMetadata.IsRemote;
            }

            public void LoadBundle()
            {
                if (!IsLoaded)
                    assetBundle = AssetBundle.LoadFromFile(bundlePath);
            }

            public void UnloadBundle()
            {
                if (IsLoaded)
                    assetBundle = null;
            }

            public string[] GetDependencies()
            {
                return bundleMetadata.Dependecies;
            }

            public string[] GetAssetNames()
            {
                return bundleMetadata.Assets;
            }
        }

    }
}
