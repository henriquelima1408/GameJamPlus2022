using System;
using UnityEngine;
using UnityFx.Async;
using UnityFx.Async.Promises;
using System.Collections.Generic;

namespace App.Game.Services
{
    public class LevelSelectorService : ILevelSelectorService
    {
        public bool IsInitialized => isInitialized;
        public LevelData SelectedLevelData => levelDatas[currentLevelDataIndex];
        public IReadOnlyCollection<LevelData> LevelDatas => levelDatas;

        LevelData[] levelDatas;
        int currentLevelDataIndex;
        IBundleService bundleService;
        const string levelsBundleName = "level-data";
        bool isInitialized;

        public LevelSelectorService(IAsyncOperation<IBundleService> bundleServicePromisse)
        {
            bundleServicePromisse.Then((b) =>
            {
                bundleService = b;
                var levelNames = bundleService.GetAssetNames(levelsBundleName);

                levelDatas = new LevelData[levelNames.Length];

                for (int i = 0; i < levelDatas.Length; i++)
                {
                    bundleService.LoadAsset<LevelData>(levelsBundleName, levelNames[i]).Then((levelData) =>
                    {
                        levelDatas[i] = levelData;
                    }).Catch((e) => Debug.LogException(e));
                }

                isInitialized = true;
            }).Catch((e) => Debug.LogException(e));
        }

        public void Dispose()
        {

            levelDatas = null;
        }

        public void SetLevel(LevelData levelData)
        {
            var index = Array.IndexOf(levelDatas, levelData);

            if (index != -1)
            {
                currentLevelDataIndex = index;
            }
        }

        public void SetLevel(int index)
        {
            if (index < 0 || index > levelDatas.Length - 1)
            {
                Debug.Log($"Index with value {index} dont represent bounds of level data collection");
                return;
            }

            currentLevelDataIndex = index;
        }
    }
}
