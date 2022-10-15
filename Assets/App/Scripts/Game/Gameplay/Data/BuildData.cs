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

        public string Id { get => id; }
        public GameObject Prefab { get => prefab; }
        public bool IsStaticBuild { get => isStaticBuild; }
        public int UpdateFrequency { get => updateFrequency; }
        public int AreaRadius { get => areaRadius; }
        public BuildPatternType BuildPattern { get => buildPattern; }
    }
    public enum BuildPatternType
    {
        AllDirections
    }
}