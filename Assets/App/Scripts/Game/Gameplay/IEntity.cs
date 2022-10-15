using System;
using System.Collections;
using UnityEngine;

namespace App.Game.Gameplay
{
    public interface IEntity
    {
        string GUID { get; }
        Action<IEntity> OnDestroy { get; set; }
        Action<IEntity> OnBuild { get; set; }
        Action<IEntity> OnTakeDamage { get; set; }        
        Action<IEntity> OnUpdate { get; set; }
        void Update();
    }
}