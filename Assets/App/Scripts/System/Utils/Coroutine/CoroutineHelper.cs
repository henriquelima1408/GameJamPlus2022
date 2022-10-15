using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace App.System.Utils
{
    public class CoroutineHelper : MonoSingleton<CoroutineHelper>
    {
        Dictionary<string, IEnumerator> coroutineDict;

        protected override void Dispose()
        {
            coroutineDict = null;
        }

        protected override void Init()
        {
            coroutineDict = new Dictionary<string, IEnumerator>();
        }

        public string AddCoroutine(IEnumerator enumerator)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(enumerator.GetHashCode());
            stringBuilder.Append(Guid.NewGuid().ToString());

            string coroutineID = stringBuilder.ToString();

            coroutineDict.Add(coroutineID, enumerator);

            StartCoroutine(enumerator);

            return coroutineID;
        }
        public void RemoveCoroutine(string coroutineID)
        {
            StopCoroutine(coroutineDict[coroutineID]);
            coroutineDict.Remove(coroutineID);
        }
    }
}