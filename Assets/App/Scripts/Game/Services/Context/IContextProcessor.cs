using System;

namespace App.Game.Services
{
    public interface IContextProcessor : IDisposable
    {
        void Apply();
    }
}
