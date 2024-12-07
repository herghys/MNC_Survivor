using UnityEngine;

namespace HerghysStudio.Survivor.Utility.Singletons
{
    public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        public void Awake()
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
            DoOnAwake();
        }

        public virtual void DoOnAwake() { }
    }
}