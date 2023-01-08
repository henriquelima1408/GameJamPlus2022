using App.Game.Data;
using App.Game.Gameplay;
using App.System.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Game.WorldBuild
{
    public class Cell
    {
        readonly Vector2Int cellPos;
        readonly GameObject self;
        readonly SpriteRenderer spriteRenderer;
        bool isEditable;
        bool canBeUsedToPath;
        bool isInPreviewState;
        bool isStatic = false;

        Sprite initialSprite;

        IBuild build;
        GameObject buildGameObject;

        public int X { get => cellPos.x; }
        public int Y { get => cellPos.y; }
        public int Cost { get; set; }
        public Cell Parent { get; set; }
        public int CostDistance => Cost + Distance;
        public int Distance { get; set; }
        public void SetDistance(int targetX, int targetY)
        {
            this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }


        public Cell(Vector2Int cellPos, GameObject self, bool isEditable, bool canBeUsedToPath, bool isStatic)
        {
            this.cellPos = cellPos;
            this.self = self;
            this.isEditable = isEditable;
            this.spriteRenderer = self.GetComponent<SpriteRenderer>();
            this.canBeUsedToPath = canBeUsedToPath;
            initialSprite = spriteRenderer.sprite;
            this.isStatic = isStatic;   
        }

        public Vector2Int CellPos => cellPos;
        public GameObject Self => self;
        public bool IsEditable { get => isEditable; }
        public IBuild Build { get => build; }
        public GameObject BuildGameObject { get => buildGameObject; }
        public bool CanBeUsedToPath { get => canBeUsedToPath; }
        public bool IsInPreviewState { get => isInPreviewState; }
        public bool IsStatic { get => isStatic; }

        public void CreateBuild(BuildData buildData, HashSet<Cell> cellsInArea, Action<IEntity> onBuild)
        {
            buildGameObject = MonoBehaviour.Instantiate(buildData.Prefab);
            buildGameObject.transform.parent = self.transform;
            buildGameObject.transform.localPosition = Vector2.zero;
            buildGameObject.SetActive(false);

            string guid = Guid.NewGuid().ToString();
            build = new DynamicBuild(guid, this, buildData, buildGameObject, cellsInArea);
            //build.OnBuild += onBuild;
            isEditable = false;
            canBeUsedToPath = true;
            isInPreviewState = true;

            foreach (var c in cellsInArea)
            {
                if (c != null)
                {
                    c.isInPreviewState = true;
                }
            }

            Debug.Log($"build {guid} finish");

            buildGameObject.SetActive(true);
            //build.OnBuild?.Invoke(build);
        }

        public void Select(bool isHover)
        {
            if (!isEditable) return;            

            if (!isHover)
            {
                isEditable = false;
                canBeUsedToPath = true;
                spriteRenderer.sprite = LevelController.Instance.NatureTile;
            }
            else
            {
                spriteRenderer.sprite = LevelController.Instance.PreviewTile;
            }
        }

        public void Deselect()
        {
            if (!isEditable) return;

            spriteRenderer.sprite = initialSprite;
            isInPreviewState = false;
        }

        public void RemoveBuild()
        {
            build.Destroy();
            build = null;
        }
    }
}
