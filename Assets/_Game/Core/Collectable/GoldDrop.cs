using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Character;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Collectable
{
    public class GoldDrop : MonoBehaviour
    {
        [SerializeField] LayerMask layers;
        private IObjectPool<GoldDrop> _pool;

        public void Setup(IObjectPool<GoldDrop> pool)
        {
            _pool = pool;
        }

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.TryGetComponent<PlayerController>(out var p);
            if (p != null)
            {
                GameManager.Instance.GoldCount++;
                _pool.Release(this);
            }
        }
    }
}
