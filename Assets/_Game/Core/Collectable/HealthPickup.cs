using System.Collections;
using System.Collections.Generic;
using HerghysStudio.Survivor.Character;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Collectables
{
    public class HealthPickup : Collectable
    {
        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.TryGetComponent<PlayerController>(out var p);
            if (p != null)
            {
                p.OnReceiveHealth(CollectableValue);
                _pool.Release(this);
            }
        }
    }
}
