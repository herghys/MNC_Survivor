using UnityEngine;

namespace HerghysStudio.Survivor.Utility.Singletons
{
    public abstract class NonPersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;
        public virtual void Awake()
        {
            Instance = this as T;
            DoOnAwake();
        }

        public virtual void DoOnAwake()
        {

        }
    }
}