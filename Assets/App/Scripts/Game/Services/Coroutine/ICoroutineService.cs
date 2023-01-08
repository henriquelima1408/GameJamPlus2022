using System.Collections;
using static App.System.Utils.CoroutineService;

namespace App.Game.Services
{
    public interface ICoroutineService : IService
    {
        CoroutineInfo AddCoroutine(IEnumerator enumerator);
        void RemoveCoroutine(string coroutineID);
    }
}
