using System;
using UnityEngine;

namespace App.Game.Services
{
    [Serializable]
    public struct ContextFilter
    {
        [SerializeField] Context context;
        [SerializeField] Context notAllowedContexts;
        [SerializeField] Context dependentContexts;

        public Context Context { get => context; }
        public Context NotAllowedContexts { get => notAllowedContexts;}
        public Context DependentContexts { get => dependentContexts;}
    }
}
