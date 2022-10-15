namespace App.System.Utils
{
    using UnityEngine;
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject g = new GameObject(typeof(T).Name);
                    instance = g.AddComponent<T>();
                    instance.Init();
                    return instance;
                }

                return instance;
            }
            protected set
            {
                instance?.Dispose();

                instance = value;                
                instance?.Init();
            }
        }

        protected abstract void Init();
        protected abstract void Dispose();
    }
}
