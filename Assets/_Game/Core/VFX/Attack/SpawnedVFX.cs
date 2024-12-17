using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace HerghysStudio.Survivor.VFX
{
    public class SpawnedVFX : AttackVFX
    {
        [SerializeField] protected bool UseParticleCollision;
        [SerializeField] protected bool DamageOnStop;

        private void OnParticleCollision(GameObject other)
        {
            if (!isActivated)
                return;

            if (UseParticleCollision)
            {
                CheckDamageCollision();
            }
        }

        private void OnParticleSystemStopped()
        {
            if (!isActivated)
                return;

            if (DamageOnStop)
            {
                CheckDamageCollision();
            }

            ReleaseToPool();

            if (GameManager.Instance.IsPlayerDead)
                particle.Stop();
        }

        public override void SetupTarget(Transform target, Vector3 targetPosition, Transform parent, VFXOwner owner)
        {
            Target = target;
            OwnerTransform = parent;

            if (isSpawnOnRandomPosition)
                TargetPosition = targetPosition;

            if (isAttackFromOwner)
                TargetPosition = OwnerTransform.position;
        }

        private void Update()
        {
            if (isAttackFromOwner)
                TargetPosition = OwnerTransform.position;

            transform.position = TargetPosition;
        }

        public override void StartLogic()
        {
            base.StartLogic();
        }

        protected override void Activate()
        {
            base.Activate();
        }

        protected override float Damage()
        {
            if (collisionDivideByDuration)
                return base.Damage() / particle.main.duration;
            else
                return base.Damage();
        }
    }
}
