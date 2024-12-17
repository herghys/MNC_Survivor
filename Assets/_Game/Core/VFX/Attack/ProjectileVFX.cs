using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Character;

using UnityEngine;

namespace HerghysStudio.Survivor.VFX
{
    public class ProjectileVFX : AttackVFX
    {
        private void Update()
        {
            if (GameManager.Instance.IsPlayerDead)
                return;

            if (_currentAttackBehaviour == null)
                return;

            if (isActivated)
                return;

            if (isHoming)
                HomingLogic();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == layerMask)
            {
                CheckDamageCollision();
            }
            ReleaseToPool();

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == layerMask)
            {
                CheckDamageCollision();
            }

            ReleaseToPool();

        }


        public override void SetupTarget(Transform target, Vector3 targetPosition, Transform parent, VFXOwner owner)
        {
            Target = target;
            OwnerTransform = parent;
        }

        public override void StartLogic()
        {
            base.StartLogic();
        }

        protected override void Activate()
        {
            base.Activate();
            if (!isHoming)
            {
                rigidBody.velocity = (OwnerTransform.rotation * Vector3.forward) * skill.ProjectileSpeed;
            }
        }

        private void HomingLogic()
        {
            var pos = Vector3.MoveTowards(transform.position, Target.position, skill.ProjectileSpeed);
            rigidBody.MovePosition(pos);
        }
    }
}
