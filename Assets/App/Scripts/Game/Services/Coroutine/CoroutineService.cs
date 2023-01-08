using App.Game.Services;
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
        public class CoroutineInfo {

            readonly IAsyncOperation<string> asyncOperation;
            readonly string coroutineID;

            public CoroutineInfo(string coroutineID, IAsyncOperation<string> asyncOperation)
            {
                this.asyncOperation = asyncOperation;
                this.coroutineID = coroutineID;
            }

            public IAsyncOperation<string> AsyncOperation => asyncOperation;

            public string CoroutineID => coroutineID;
        }



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

        public CoroutineInfo AddCoroutine(IEnumerator enumerator)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(enumerator.GetHashCode());
            stringBuilder.Append(Guid.NewGuid().ToString());

            var coroutineID = stringBuilder.ToString();

            var asyncCompletionSource = new AsyncCompletionSource<string>();
            coroutineDict.Add(coroutineID, DoCoroutine(enumerator, coroutineID, asyncCompletionSource));

            StartCoroutine(enumerator);

            return new CoroutineInfo(coroutineID, asyncCompletionSource);
        }

        public void RemoveCoroutine(string coroutineID)
        {
            StopCoroutine(coroutineDict[coroutineID]);
            coroutineDict.Remove(coroutineID);
        }

        IEnumerator DoCoroutine(IEnumerator coroutine, string coroutineID, IAsyncCompletionSource<string> onFinish)
        {
            yield return coroutine;
            onFinish?.SetResult(coroutineID);
        }
    }
}