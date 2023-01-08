using System;

namespace App.Game.Gameplay
{
    public abstract class TimeEntity : IEntity
    {
        readonly float updateFrequency;
        readonly string guid;
        float currentTime;

        public string GUID => guid;

        public event Action OnUpdate;
        public TimeEntity(float updateFrequency)
        {
            this.updateFrequency = updateFrequency;
            guid = new Guid().ToString();
        }

        public virtual void DecreaseTime(float time) {

            currentTime -= time;

            if (currentTime <= 0) {

                currentTime = updateFrequency;
                OnUpdate?.Invoke();            
            }
        }

        protected virtual void Dimiss()
        {
            OnUpdate = null;
        }
    }
}
