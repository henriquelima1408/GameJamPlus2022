using App.Game.Services.LevelServiceMock;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Game.Services
{
    public class LevelSelectorService : ILevelSelectorService
    {
        LevelData[] levelDatas;
        int currentLevelDataIndex;

        public bool IsInitialized => true;
        public LevelData SelectedLevelData => levelDatas[currentLevelDataIndex];
        public IReadOnlyCollection<LevelData> LevelDatas => levelDatas;

        public LevelSelectorService(LevelData[] levelDatas)
        {
            Debug.Assert(levelDatas != null && levelDatas.Length > 0, "LevelDatas is invalid");
            this.levelDatas = levelDatas;
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
