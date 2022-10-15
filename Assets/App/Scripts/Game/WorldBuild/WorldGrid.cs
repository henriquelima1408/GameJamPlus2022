using UnityEngine;
using App.Game.Data;

namespace App.Game.WorldBuild
{
    public class WorldGrid
    {
        readonly Cell[,] cells;
        readonly Vector2Int gridSize;
        readonly Vector2 cellSpriteSize;
        readonly Vector2 cellCollisionSize;
        readonly TileMapData tileMapData;
        readonly Vector2 gridSpawnPoint;
        readonly Transform gridParent;

        public WorldGrid(Vector2Int gridSize, TileMapData tileMapData, Transform gridParent, Vector2 gridSpawnPoint)
        {
            this.gridSize = gridSize;
            this.tileMapData = tileMapData;
            this.gridSpawnPoint = gridSpawnPoint;
            this.gridParent = gridParent;

            SpriteRenderer spriteRenderer = tileMapData.TilePrefab.GetComponent<SpriteRenderer>();
            this.cellSpriteSize = spriteRenderer.size;

            BoxCollider2D boxCollider2D = tileMapData.TilePrefab.GetComponent<BoxCollider2D>();
            this.cellCollisionSize = boxCollider2D.size;


            this.cells = new Cell[gridSize.x, gridSize.y];

            SpawnGrid();
        }

        void SpawnGrid()
        {
            Vector2 spawnPos;
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    spawnPos = gridSpawnPoint + new Vector2(x * cellSpriteSize.x, y * cellSpriteSize.y);

                    GameObject cell = MonoBehaviour.Instantiate(tileMapData.TilePrefab);
                    cell.transform.position = spawnPos;
                    cell.transform.parent = gridParent;
                    cell.name = $"Cell {x} - {y}";

                    SpriteRenderer cellSpriteRenderer = cell.GetComponent<SpriteRenderer>();
                    cellSpriteRenderer.sprite = FindTileSprite(gridSize, new Vector2Int(x, y));
                    cellSpriteRenderer.flipX = x == 0;

                    cells[x, y] = new Cell(new Vector2Int(x, y), cell, true);
                }
            }
        }

        Sprite FindTileSprite(Vector2Int gridSize, Vector2Int currentPoint)
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
    }
}
