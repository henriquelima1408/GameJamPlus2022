using UnityEngine;

namespace App.Game.Data
{
    [CreateAssetMenu(fileName = "GameplayDatasheet", menuName = "Gameplay/Data/GameplayDatasheet")]
    public class GameplayDatasheet : ScriptableObject
    {
        [SerializeField]
        TileMapData[] tileMapDatas;

        [SerializeField]
        Vector2Int gridSize;

        [SerializeField]
        float playerCameraSpd;

        //TODO: Refactor to be possible to split builds between factions
        [SerializeField]
        BuildData[] buildDatas;

        public Vector2Int GridSize { get => gridSize; }
        public TileMapData[] TileMapDatas { get => tileMapDatas; }
        public float PlayerCameraSpd { get => playerCameraSpd; }
        public BuildData[] BuildDatas { get => buildDatas; }
    }
}
