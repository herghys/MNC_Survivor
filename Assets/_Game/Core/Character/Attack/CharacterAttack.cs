using System;
using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.VFX;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Character
{
    public abstract class CharacterAttack : MonoBehaviour
    {
        public BasicAttackSkill BasicAttackSkill { get; private set; }

        protected Transform Target;

        [SerializeField] protected Transform vfxHolder;

        protected ObjectPool<AttackVFX> vfxPool;

        private void Awake()
        {
            DoOnAwake();   
        }

        protected abstract void DestroVFX(AttackVFX vFX);
        protected abstract void ReleaseVFX(AttackVFX vFX);
        protected abstract void GetVFX(AttackVFX vFX);
        protected abstract AttackVFX CreateVFX();

        protected virtual void DoOnAwake() { }

        public virtual void Setup (BasicAttackSkill skill, Transform target)
        {
            BasicAttackSkill = skill;
            Target = target;
        }

        public abstract void BasicAttackTarget(BasicAttackSkill skill);

        public abstract IEnumerator IEBasicAttack(BasicAttackSkill skill);
    }
}
