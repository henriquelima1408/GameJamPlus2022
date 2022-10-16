using System.Collections;
using UnityEngine;
using App.System.Utils;
using System;
using static App.Game.WorldBuild.WorldGrid;

namespace App.Game.Gameplay
{
    public class LevelController : MonoSingleton<LevelController>
    {
        [SerializeField]
        LevelData[] levelDatas;

        [SerializeField]
        int currentLevelDataIndex;

        [SerializeField]
        GameObject destinationPrefab;

        [SerializeField]
        GameObject stonePrefab;       

        private void Awake()
        {
            if (GetInstance() != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        protected override void Dispose()
        {

        }

        public LevelData LevelData => levelDatas[currentLevelDataIndex];        

        public void SetNextLevel()
        {
            if (currentLevelDataIndex + 1 < levelDatas.Length)
            {
                currentLevelDataIndex++;
            }
        }

        public void SetNextLevel(int index)
        {
            currentLevelDataIndex = index;
        }

        protected override void Init()
        {
            DontDestroyOnLoad(gameObject);
        }

        public GameObject GetTileAsset(TileType cellType)
        {
            if (cellType == TileType.Stone)
                return stonePrefab;

            if (cellType == TileType.Destination)
                return destinationPrefab;


            return null;
        }


    }
}