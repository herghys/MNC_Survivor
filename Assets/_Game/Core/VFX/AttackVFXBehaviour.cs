using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace HerghysStudio.Survivor.VFX
{
    [Serializable]
    public class AttackVFXBehaviour
    {
        [Tooltip("Hold time on parent")]
        public float Delay;
        public float ActiveTime = 5f;
        public bool Activate;

        public AttackVFXBehaviour() { }

        public AttackVFXBehaviour(AttackVFXBehaviour b)
        {
            Delay = b.Delay;
            ActiveTime = b.ActiveTime;
            Activate = b.Activate;
        }
    }

    public enum VfxBehaviour
    {
        Hold,
        MoveToTarget,
        SpawnOnSelf,
        SpawnDirectly,
        SpawnDurectlyRandom
    }
}
