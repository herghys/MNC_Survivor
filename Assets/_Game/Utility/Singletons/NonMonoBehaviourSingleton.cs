using System;

namespace HerghysStudio.Survivor.Utility.Singletons
{
    public class NonMonoBehaviourSingleton<T> where T : class, new()
    {
        #region Lazy
        protected static readonly Lazy<T> instance = new Lazy<T>(() => new T());

        public static T Instance { get { return instance.Value; } }

        public NonMonoBehaviourSingleton() { }
        #endregion
    }
}
