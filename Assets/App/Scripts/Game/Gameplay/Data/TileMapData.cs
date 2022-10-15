using System;
using UnityEngine;

namespace App.Game.Data
{
    [Serializable]
    public class TileMapData
    {
        [SerializeField]
        GameObject tilePrefab;

        [SerializeField]
        Sprite midBottomTile;

        [SerializeField]
        Sprite sideBottonTile;

        [SerializeField]
        Sprite midTile;

        [SerializeField]
        Sprite topTile;

        [SerializeField]
        Sprite sideTile;

        [SerializeField]
        Sprite topSideTile;

        public Sprite MidBottomTile { get => midBottomTile; }
        public Sprite SideBottonTile { get => sideBottonTile; }
        public Sprite MidTile { get => midTile; }
        public Sprite TopTile { get => topTile; }
        public Sprite SideTile { get => sideTile; }
        public Sprite TopSideTile { get => topSideTile; }
        public GameObject TilePrefab { get => tilePrefab;}
    }
}
