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
            LoadBundle(bundleName, new AsyncCompletionSource()).
                Then(() =>
                {
                    var asset = bundles[bundleName].AssetBundle.LoadAsset<T>(assetName);
                    resultOperation?.SetResult(asset);
                });

            return resultOperation;
        }

        public IAsyncOperation LoadBundle(string bundleName, IAsyncCompletionSource completionSource)
        {
            var operation = new AsyncCompletionSource();
            var bundle = bundles[bundleName];

            if (!bundle.IsLoaded)
            {
                Debug.Assert(IsBundleReady(bundleName), $"Bundle {bundleName} isnt ready");
                bundle.LoadBundle();
            }

            completionSource?.SetCompleted();
            return operation;
        }

        public bool IsBundleReady(string bundleName)
        {
            return bundles[bundleName].IsReady;
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
            public bool IsLoaded { get => assetBundle != null; }
            public bool IsReady { get => !IsRemote; }
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
        }

    }

    [Serializable]
    class BundleManifest
    {
        BundleMetadata[] bundleMetadatas;

        public BundleManifest(BundleMetadata[] bundleMetadatas)
        {
            this.bundleMetadatas = bundleMetadatas;
        }

        public BundleMetadata[] BundleMetadatas { get => bundleMetadatas; }
    }

    [Serializable]
    public class BundleMetadata
    {
        readonly string bundleName;
        readonly string[] dependecies;
        readonly string[] assets;
        readonly double version;
        readonly double size;
        readonly bool isRemote;

        public BundleMetadata(string bundleName, string[] dependecies, string[] assets, double version, double size, bool isRemote)
        {
            this.bundleName = bundleName;
            this.dependecies = dependecies;
            this.assets = assets;
            this.version = version;
            this.size = size;
            this.isRemote = isRemote;
        }

        public string BundleName => bundleName;

        public string[] Dependecies => dependecies;

        public string[] Assets => assets;

        public double Version => version;

        public double Size => size;

        public bool IsRemote => isRemote;
    }


    //public static class A
    //{


    //    [UnityEditor.MenuItem("Jobs/Test path")]
    //    public static void T()
    //    {
    //        string fileName = "BundleManifest.txt";
    //        var bundleManifestPath = Path.Combine(Application.streamingAssetsPath, "BundleManifest", $"{fileName}");


    //        using (StreamWriter streamReader = new StreamWriter(bundleManifestPath))
    //        {
    //            streamReader.Write("xablauuu");
    //        }

    //        //if (!File.Exists(bundleManifestPath))
    //        //{

    //        //    File.Create(bundleManifestPath);

    //        //}

    //    }


    //}
}
