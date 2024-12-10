using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Collectables
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] protected LayerMask layers;

        protected IObjectPool<Collectable> _pool;
        [field: SerializeField] public float CollectableValue { get; private set; }

        public virtual void Setup(IObjectPool<Collectable> pool)
        {
            _pool = pool;
        }
    }
}
