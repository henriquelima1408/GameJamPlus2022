

using System;

namespace App.Game.Services
{
    public interface IContextService : IService
    {
        public Context Context { get; }

        public event Action<Context> OnContextChange;
        void AddContext(Context ctx);
        void RemoveContext(Context ctx);
    }

    [Flags]
    [Serializable]
    public enum Context
    {
        None  = 0,
        TitleScreen = 1,
        LevelScreen = 2,
    }
}
