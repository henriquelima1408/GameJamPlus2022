using App.System.Utils;
using System;

namespace App.Game.Gameplay
{
    public interface ITurnUpdater : IDisposable
    {
        float UpdateFrequency { get; }
        void DoUpdate(float time);
        void OnTimeFinished();
    }
}
