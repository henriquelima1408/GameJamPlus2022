using System;

namespace App.Game.Services
{
    public interface IUpdaterService : IService
    {
        public event Action OnUpdate;
        public event Action OnLateUpdate;
    }
}
