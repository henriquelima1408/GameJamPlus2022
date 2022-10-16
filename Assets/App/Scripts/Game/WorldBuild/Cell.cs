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

        IBuild build;
        GameObject buildGameObject;

        public Cell(Vector2Int cellPos, GameObject self, bool isEditable)
        {
            this.cellPos = cellPos;
            this.self = self;
            this.isEditable = isEditable;
            this.spriteRenderer = self.GetComponent<SpriteRenderer>();
        }

        public Vector2Int CellPos => cellPos;
        public GameObject Self => self;
        public bool IsEditable { get => isEditable; }
        public IBuild Build { get => build; }
        public GameObject BuildGameObject { get => buildGameObject; }


        public void CreateBuild(BuildData buildData, HashSet<Cell> cellsInArea, Action<IEntity> onBuild)
        {
            buildGameObject = MonoBehaviour.Instantiate(buildData.Prefab);
            buildGameObject.transform.parent = self.transform;
            buildGameObject.transform.localPosition = Vector2.zero;
            buildGameObject.SetActive(false);

            string guid = Guid.NewGuid().ToString();
            build = new DynamicBuild(guid, this, buildData, buildGameObject, cellsInArea);
            build.OnBuild += onBuild;
            isEditable = false;

            Debug.Log($"build {guid} finish");

            buildGameObject.SetActive(true);
            build.OnBuild?.Invoke(build);
        }

        public void Select(bool isHover)
        {
            if (!isEditable) return;

            if (!isHover)
            {
                isEditable = false;
                spriteRenderer.color = Color.blue;
            }
            else
            {
                spriteRenderer.color = Color.red;
            }
        }

        public void Deselect()
        {
            if (!isEditable) return;
            spriteRenderer.color = Color.white;
        }

        public void RemoveBuild()
        {
            build.Destroy();
            build = null;
        }
    }
}
