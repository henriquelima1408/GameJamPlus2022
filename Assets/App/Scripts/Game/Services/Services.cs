using App.Game.Services.CoroutineServiceMock;
using App.Game.Services.LevelServiceMock;
using App.Game.Services.SoudServiceMock;
using App.System.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFx.Async;
using UnityFx.Async.Promises;

namespace App.Game.Services
{
    public class Services : MonoBehaviour, IService
    {
        static Services instance;
        public static Services Instance { get => instance; }


        readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Init();
            StartServices();
        }

        void Init()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public bool IsInitialized
        {
            get
            {
                foreach (var service in services.Values)
                {
                    if (!service.IsInitialized) return false;
                }
                return true;
            }
        }

        public void Dispose()
        {
            foreach (var service in services.Values)
            {
                service.Dispose();
            }

        }

        public void StartServices()
        {
            var coroutineHelper = new GameObject("CoroutineHelper").AddComponent<CoroutineService>();
            coroutineHelper.transform.parent = transform;
            services.Add(typeof(ICoroutineService), coroutineHelper);

            services.Add(typeof(ISoundService), new SoundService(GetService<ICoroutineService>()));
            services.Add(typeof(ILevelSelectorService), new LevelSelectorService(null));
        }

        public IAsyncOperation<T> GetService<T>()
        {
            var serviceType = typeof(T);
            if (!services.ContainsKey(serviceType))
            {
                Debug.LogError($"Services do not contains a service with from type: {serviceType}.");
                return null;
            }

            var service = services[serviceType];
            var serviceRequest = new AsyncCompletionSource<T>();

            if (service.IsInitialized)
            {
                serviceRequest.SetResult((T)service);
            }
            else
            {
                var coroutineHelper = GetService<CoroutineService>().Result as CoroutineService;

                coroutineHelper.AddCoroutine(ServiceRequest(serviceRequest, services[typeof(T)])).
                    Then((coroutineID) => coroutineHelper.RemoveCoroutine(coroutineID));
            }

            return serviceRequest;
        }

        IEnumerator ServiceRequest<T>(IAsyncCompletionSource<T> asyncOperation, IService service)
        {
            while (!service.IsInitialized)
            {
                yield return null;
            }

            asyncOperation.SetResult((T)service);
        }
    }
}
