using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.App.Scripts.System.Utils
{
    public static class ExtensionMethods
    {
        public static T GetComponentInRootObjects<T>(this Scene scene, bool recursive = false) where T : MonoBehaviour
        {
            if (!scene.IsValid())
                throw new Exception("Invalid scene");

            var gameObjects = scene.GetRootGameObjects();

            foreach (var gameObject in gameObjects)
            {
                T component;
                if (recursive)
                {
                    component = gameObject.GetComponentInChildren<T>(true);
                }
                else
                {
                    component = gameObject.GetComponent<T>();
                }

                if (component != null)
                {
                    return component;
                }
            }

            return null;

        }
    }
}
