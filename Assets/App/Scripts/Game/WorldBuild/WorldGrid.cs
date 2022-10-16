using UnityEngine;
using App.Game.Data;
using System.Collections.Generic;
using App.Game.Gameplay;

namespace App.Game.WorldBuild
{
    public class WorldGrid
    {
        readonly Cell[,] cells;
        readonly Vector2Int gridSize;
        readonly Vector2 cellSpriteSize;
        readonly Vector2 cellCollisionSize;
        readonly TileMapData[] tileMapDatas;
        readonly LevelController levelController;
        readonly Vector2 gridSpawnPoint;
        readonly Transform gridParent;
        readonly HashSet<Cell> destinationCells = new HashSet<Cell>();
        readonly Dictionary<Color, TileType> textureDescriptor = new Dictionary<Color, TileType>
        {
            { Color.white,TileType.Wasteland },
            { Color.black,TileType.Stone  },
            { Color.red,TileType.Destination },
            { Color.green,TileType.None },
            { Color.blue,TileType.Enemy },
        };

        public Vector2Int GridSize => gridSize;

        public HashSet<Cell> DestinationCells => destinationCells;

        public enum TileType
        {
            None = 0,
            Wasteland = 1,
            Stone = 2,
            Destination = 3,
            Enemy =4
        }

        public WorldGrid(TileMapData[] tileMapData, Transform gridParent, Vector2 gridSpawnPoint)
        {
            var mapTexture = LevelController.Instance.LevelData.MapTexture;

            this.levelController = LevelController.Instance;
            this.gridSize = new Vector2Int(mapTexture.width, mapTexture.height);
            this.tileMapDatas = tileMapData;
            this.gridSpawnPoint = gridSpawnPoint;
            this.gridParent = gridParent;

            BoxCollider2D boxCollider2D = tileMapData[0].TilePrefab.GetComponent<BoxCollider2D>();
            this.cellCollisionSize = boxCollider2D.size;

            SpriteRenderer spriteRenderer = tileMapData[0].TilePrefab.GetComponent<SpriteRenderer>();
            this.cellSpriteSize = cellCollisionSize;

            this.cells = new Cell[gridSize.x, gridSize.y];

            SpawnGrid(mapTexture);
        }

        void SpawnGrid(Texture2D texture2D)
        {
            Vector2 spawnPos;
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    spawnPos = gridSpawnPoint + new Vector2(x * cellSpriteSize.x, y * cellSpriteSize.y);

                    var c = texture2D.GetPixel(x, y);

                    var cellType = TileType.None;
                    textureDescriptor.TryGetValue(c, out cellType);

                    var tileMapData = tileMapDatas[(int)cellType];

                    GameObject cellObj = MonoBehaviour.Instantiate(tileMapData.TilePrefab);
                    cellObj.transform.position = spawnPos;
                    cellObj.transform.parent = gridParent;
                    cellObj.name = $"Cell {x} - {y}";

                    SpriteRenderer cellSpriteRenderer = cellObj.GetComponent<SpriteRenderer>();
                    cellSpriteRenderer.sprite = FindTileSprite(tileMapData, gridSize, new Vector2Int(x, y));
                    cellSpriteRenderer.flipX = x == 0;


                    var cell = new Cell(new Vector2Int(x, y), cellObj, cellType == TileType.Wasteland, cellType == TileType.Destination, cellType == TileType.None);
                    cells[x, y] = cell;

                    var destinationFlip = false;
                    if (cellType == TileType.Destination)
                    {
                        destinationCells.Add(cell);
                        destinationFlip = destinationCells.Count == 2;
                    }

                    if (cellType == TileType.Stone || cellType == TileType.Destination || cellType == TileType.Enemy)
                    {
                        var prefab = levelController.GetTileAsset(cellType);
                        var asset = MonoBehaviour.Instantiate(prefab, cell.Self.transform);
                        var sprite = asset.GetComponent<SpriteRenderer>();

                        sprite.sortingOrder = 1;
                        if (destinationFlip)
                        {
                            sprite.flipX = true;
                        }
                    }

                }
            }
        }


        Sprite FindTileSprite(TileMapData tileMapData, Vector2Int gridSize, Vector2Int currentPoint)
        {
            bool isSide = currentPoint.x == 0 || currentPoint.x == gridSize.x - 1;
            bool isTop = currentPoint.y == gridSize.y - 1;
            bool isBottom = currentPoint.y == 0;

            if (isBottom && isSide)
            {
                return tileMapData.SideBottonTile;
            }
            else if (isTop && isSide)
            {
                return tileMapData.TopSideTile;
            }
            else if (isSide)
            {
                return tileMapData.SideTile;
            }
            else if (isBottom)
            {
                return tileMapData.MidBottomTile;
            }
            else if (isTop)
            {
                return tileMapData.TopTile;
            }
            else
            {
                return tileMapData.MidTile;
            }
        }

        public Cell GetCellInPosition(Vector2 pos)
        {
            Vector2 halfCellSize = cellSpriteSize / 2;
            Vector2 deltaVec = pos - (halfCellSize * -1 + gridSpawnPoint);

            if (deltaVec.x < 0 || deltaVec.y < 0) return null;

            int x = (int)(deltaVec.x / halfCellSize.x) / 2;
            int y = (int)(deltaVec.y / halfCellSize.y) / 2;

            if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y) return null;

            return cells[x, y];
        }

        public Cell GetCellInDirection(Vector2Int startPos, Vector2Int dir)
        {
            var deltaPos = startPos + dir;
            if (deltaPos.x < 0 || deltaPos.x >= gridSize.x || deltaPos.y < 0 || deltaPos.y >= gridSize.y) return null;

            return cells[deltaPos.x, deltaPos.y];
        }

        public Cell GetCell(Vector2Int pos)
        {
            return cells[pos.x, pos.y];
        }
    }
}
