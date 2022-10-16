using App.Game.WorldBuild;
using App.Game.Data;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace App.Game.Gameplay
{
    public class DynamicBuild : IBuild
    {
        readonly string guid;
        readonly Cell cell;
        readonly BuildData buildData;
        readonly GameObject self;
        readonly HashSet<Cell> cellsInArea;

        Action<IEntity> onUpdate;
        Action<IEntity> onDestroy;
        Action<IEntity> onBuild;
        Action<IEntity> onTakeDamage;

        public DynamicBuild(string guid, Cell cell, BuildData buildData, GameObject self, HashSet<Cell> cellsInArea)
        {
            this.guid = guid;
            this.cell = cell;
            this.buildData = buildData;
            this.self = self;
            this.cellsInArea = new HashSet<Cell>(cellsInArea);
            
        }

        public string GUID => guid;
        public Cell Cell => cell;
        public BuildData BuildData => buildData;
        public GameObject Self => self;
        public Action<IEntity> OnUpdate { get => onUpdate; set => onUpdate = value; }
        public Action<IEntity> OnDestroy { get => onDestroy; set => onDestroy = value; }
        public Action<IEntity> OnBuild { get => onBuild; set => onBuild = value; }
        public Action<IEntity> OnTakeDamage { get => onTakeDamage; set => onTakeDamage = value; }
        public IEnumerable<Cell> CellsInArea { get => cellsInArea; }

        public void Destroy()
        {
            Debug.Log($"Build with {guid} is destroying");
            onDestroy?.Invoke(this);
            MonoBehaviour.Destroy(self);

            onDestroy = null;
            onBuild = null;
            onTakeDamage = null;
            onUpdate = null;

            foreach (var cell in cellsInArea)
            {
                if (cell.IsEditable)
                {
                    cell.Deselect();
                    break;
                }
            }

        }

        public void TakeDamage(int amount)
        {
            Debug.Log($"Build with {guid} took damage");
            Destroy();
        }

        public void Update()
        {
            Debug.Log($"Build with {guid} update");

            foreach (var cell in cellsInArea)
            {
                if (cell.IsEditable) {
                    
                    cell.Select(false);
                    break;
                }
            }

            onUpdate?.Invoke(this);
        }
    }
}