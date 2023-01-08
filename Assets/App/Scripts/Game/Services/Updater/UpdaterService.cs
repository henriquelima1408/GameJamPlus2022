using System;
using UnityEngine;

namespace App.Game.Services
{
    public class UpdaterService : MonoBehaviour, IUpdaterService
    {
        public bool IsInitialized => true;

        public event Action OnUpdate;
        public event Action OnLateUpdate;

        void Update()
        {
            OnUpdate?.Invoke();
        }

        void LateUpdate() { 
            OnLateUpdate?.Invoke();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
