using App.Game.Services;
using App.Game.Services.CoroutineServiceMock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityFx.Async;

namespace App.System.Utils
{
    public class CoroutineService : MonoBehaviour, ICoroutineService
    {
        Dictionary<string, IEnumerator> coroutineDict = new Dictionary<string, IEnumerator>();

        public bool IsInitialized => true;

        public void Dispose()
        {
            coroutineDict = null;
        }

        public void Init()
        {

            DontDestroyOnLoad(gameObject);
        }

        public IAsyncOperation<string> AddCoroutine(IEnumerator enumerator)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(enumerator.GetHashCode());
            stringBuilder.Append(Guid.NewGuid().ToString());

            var coroutineID = stringBuilder.ToString();

            var asyncCompletionSource = new AsyncCompletionSource<string>();
            coroutineDict.Add(coroutineID, DoCoroutine(enumerator, coroutineID, asyncCompletionSource));

            StartCoroutine(enumerator);

            return asyncCompletionSource;
        }
        public void RemoveCoroutine(string coroutineID)
        {
            StopCoroutine(coroutineDict[coroutineID]);
            coroutineDict.Remove(coroutineID);
        }

        IEnumerator DoCoroutine(IEnumerator coroutine, string coroutineID, IAsyncCompletionSource<string> onFinish)
        {
            yield return coroutine;
            onFinish.SetResult(coroutineID);            
        }
    }
}