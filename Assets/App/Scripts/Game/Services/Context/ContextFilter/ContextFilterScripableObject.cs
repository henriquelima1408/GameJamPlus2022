using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Game.Services
{
    [CreateAssetMenu(fileName = "Context Filters", menuName = "Context Filters")]
    public class ContextFilterScripableObject : ScriptableObject
    {
        [SerializeField] ContextFilter[] contextFilters;

        public ContextFilter GetFilterByContext(Context context) {

            foreach (var contextFilter in contextFilters)
            {

                if(contextFilter.Context == context)
                    return contextFilter;
            }
            throw new Exception($"Could not find {context}");
        }
    }
}
