using System;
using UnityEngine;

namespace App.Game.Data
{
    [CreateAssetMenu(fileName = "BuildData", menuName = "Gameplay/Data/BuildData")]
    public class BuildData : ScriptableObject
    {
        [SerializeField]
        string id;

        [SerializeField]
        GameObject prefab;

        [SerializeField]
        bool isStaticBuild;

        [SerializeField]
        BuildPatternType buildPattern;

        [SerializeField]
        int areaRadius;

        [SerializeField]
        int updateFrequency;

        [SerializeField]
        Sprite buildSprite;

        public string Id { get => id; }
        public GameObject Prefab { get => prefab; }
        public bool IsStaticBuild { get => isStaticBuild; }
        public int UpdateFrequency { get => updateFrequency; }
        public int AreaRadius { get => areaRadius; }
        public BuildPatternType BuildPattern { get => buildPattern; }
        public Sprite BuildSprite { get => buildSprite; }
    }
    public enum BuildPatternType
    {
        EightDirections,
        O,
        I1,
        I2,
        S,
        S1,
        S2,
        S3,
        Z,
        L1,
        L2,
        L3,
        L4,
        L5,
        L6,
        J,
        T1,
        T2,
        T3,
        T4,
        Y1
    }
}