using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityFx.Async;

namespace App.Game.Services
{
    public interface IBundleService : IService
    {
        IAsyncOperation<T> LoadAsset<T>(string bundleName, string assetName) where T : UnityEngine.Object;
        IAsyncOperation LoadBundle(string bundleName, IAsyncCompletionSource completionSource);
        bool IsBundleReady(string bundleName);
    }
}
