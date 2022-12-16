using UnityFx.Async;

namespace App.Game.Services
{
    public interface IBundleService : IService
    {
        IAsyncOperation<T> LoadAsset<T>(string bundleName, string assetName) where T : UnityEngine.Object;
        IAsyncOperation LoadBundle(string bundleName);
        bool IsBundleReady(string bundleName);
        string[] GetAssetNames(string bundleName);
    }
}
