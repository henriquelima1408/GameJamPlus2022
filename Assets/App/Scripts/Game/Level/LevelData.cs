using App.Game.Data;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Gameplay/Data/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    Texture2D mapTexture;

    [SerializeField]
    BuildDataInfo[] buildDataInfo;

    [SerializeField]
    int maxTurnCount;

    public Texture2D MapTexture { get => mapTexture; }
    public BuildDataInfo[] BuildDataInfo { get => buildDataInfo; }
    public int MaxTurnCount { get => maxTurnCount; }
}

[System.Serializable]
public class BuildDataInfo
{
    [SerializeField]
    BuildData buildData;
    [SerializeField]
    int useCount;

    public BuildData BuildData { get => buildData; }
    public int UseCount { get => useCount; }
}