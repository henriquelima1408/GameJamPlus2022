using System.Collections;
using UnityFx.Async;

namespace App.Game.Services.CoroutineServiceMock
{
    public interface ICoroutineService : IService
    {
        IAsyncOperation<string> AddCoroutine(IEnumerator enumerator);
        void RemoveCoroutine(string coroutineID);
    }
}
