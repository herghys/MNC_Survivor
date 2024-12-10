using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace HerghysStudio.Survivor.VFX
{
    [Serializable]
    public class AttackVFXBehaviour
    {
        public VfxBehaviour VFXBehaviour;
        public bool IsHoming;
        public float Delay;
        public float ActiveTime = 5f;
        public float Speed = 1.5f;

        public AttackVFXBehaviour() { }

        public AttackVFXBehaviour(AttackVFXBehaviour b)
        {
            VFXBehaviour = b.VFXBehaviour;
            IsHoming = b.IsHoming;
            Delay = b.Delay;
            ActiveTime = b.ActiveTime;
            Speed = b.Speed;
        }
    }

    public enum VfxBehaviour
    {
        Hold,
        MoveToTarget,
        SpawnDirectly
    }
}
