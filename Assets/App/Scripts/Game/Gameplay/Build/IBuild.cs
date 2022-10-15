using App.Game.Data;
using App.Game.WorldBuild;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Game.Gameplay
{
    public interface IBuild : IEntity
    {        
        Cell Cell { get; }

        IEnumerable<Cell> CellsInArea { get; }

        BuildData BuildData { get; }
        GameObject Self { get; }        
        void Destroy();
        void TakeDamage(int amount);
        
    }
}