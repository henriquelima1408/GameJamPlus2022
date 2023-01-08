using System;
using System.Collections.Generic;
using UnityEngine;
using UnityFx.Async;
using UnityFx.Async.Promises;

namespace App.Game.Services
{
    public class ContextService : IContextService
    {
        readonly Dictionary<Context, IContextProcessor> contextProcessors = new Dictionary<Context, IContextProcessor>();

        Context context = Context.None;
        IBundleService bundleService;
        ContextFilterScripableObject contextFilterScripableObject;
        bool isInitialized;

        const string contextFilterBundleName = "scriptable-objects";
        const string contextFilterAssetName = "Context Filters";

        public ContextService(IAsyncOperation<IBundleService> bundleServicePromise)
        {
            bundleServicePromise.Then((b) =>
            {
                bundleService = b;

                var assetRequest = bundleService.LoadAsset<ContextFilterScripableObject>(contextFilterBundleName, contextFilterAssetName);
                assetRequest.Then((asset) =>
                {
                    contextFilterScripableObject = asset;

                    AddContext(Context.TitleScreen);

                    isInitialized = true;
                }).Catch((e) => Debug.LogException(e));
            }).Catch((e) => Debug.LogException(e));

        }

        public bool IsInitialized => isInitialized;
        public Context Context => context;

        public event Action<Context> OnContextChange;


        public void AddContext(Context ctx)
        {
            if (IsPossibleToAddContext(ctx))
            {
                context |= ctx;
                contextProcessors[ctx] = CreateContextProcessor(ctx);
                OnContextChange?.Invoke(ctx);
            }
        }

        public void Dispose()
        {

        }

        public void RemoveContext(Context ctx)
        {
            if (IsPossibleToRemoveContext(ctx))
            {

                context &= ctx;
                OnContextChange?.Invoke(ctx);
            }
        }

        IContextProcessor CreateContextProcessor(Context ctx)
        {
            IContextProcessor contextProcessor = null;
            switch (ctx)
            {
                case Context.None:
                    break;
                case Context.TitleScreen:
                    contextProcessor = new TitleScreenContextProcessor(Services.Instance.GetService<ISceneLoaderService>(), Services.Instance.GetService<IBundleService>());
                    break;
                case Context.LevelScreen:
                    break;
                default:
                    break;
            }

            contextProcessor.Apply();
            return contextProcessor;
        }

        void DisposeContextProcessor(Context ctx)
        {
            contextProcessors[ctx].Dispose();
            contextProcessors.Remove(ctx);
        }

        bool IsPossibleToAddContext(Context ctx)
        {
            var isContextLoaded = (context & ctx) != Context.None;
            var contextFilter = contextFilterScripableObject.GetFilterByContext(ctx);
            var thereIsAnyNotAllowedContext = (contextFilter.NotAllowedContexts & context) != Context.None;

            return !isContextLoaded && !thereIsAnyNotAllowedContext;
        }

        bool IsPossibleToRemoveContext(Context ctx)
        {
            return (context & ctx) != Context.None;
        }

    }
}
