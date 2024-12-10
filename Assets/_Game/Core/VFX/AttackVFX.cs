using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Utility.Coroutines;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.VFX
{
    public class AttackVFX : MonoBehaviour
    {
        public LayerMask LayerMask;
        public VFXOwner Owner;
        public List<AttackVFXBehaviour> Behaviours = new();
        public Transform Target;
        public AttackVFXData Data;
        public Rigidbody Rigidbody;

        public float Damage;
        public bool DirectDamage;

        public IObjectPool<AttackVFX> Pool;

        private void OnTriggerEnter(Collider other)
        {
            if (DirectDamage)
                return;


            if (other.gameObject.layer == LayerMask)
            {
               
                if (Owner == VFXOwner.Enemy)
                {
                    if (TryGetComponent<PlayerController>(out var p))
                    {
                        p.OnHit(Damage);
                    }
                }
                else
                {
                    if (TryGetComponent<EnemyController>(out var p))
                    {
                        p.OnHit(Damage);
                    }
                }

                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (DirectDamage)
                return;

            if (collision.gameObject.layer == LayerMask)
            {

                if (Owner == VFXOwner.Enemy)
                {
                    if (TryGetComponent<PlayerController>(out var p))
                    {
                        p.OnHit(Damage);
                    }
                }
                else
                {
                    if (TryGetComponent<EnemyController>(out var p))
                    {
                        p.OnHit(Damage);
                    }
                }

                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            Pool?.Release(this);
        }
        public void Setup(AttackVFXData data, Transform target, VFXOwner owner, float damage)
        {
            Data = data;
            LayerMask = data.LayerMask;
            Target = target;

            Owner = owner;
            Damage = damage;

            foreach (var item in Data.Behaviours)
            {
                Behaviours.Add(new(item));
            }
            StartVFX().Run();
        }

        IEnumerator StartVFX()
        {
            DirectDamage = false;
            yield return null;
            foreach (var b in Behaviours)
            {
                if (b.VFXBehaviour == VfxBehaviour.Hold)
                {
                    yield return new WaitForSeconds(b.Delay);
                }

                if (b.VFXBehaviour == VfxBehaviour.MoveToTarget)
                {
                    if (b.IsHoming)
                        yield return RunHoming(b);

                    else
                        yield return RunNonHoming(b);
                }

                if (b.VFXBehaviour == VfxBehaviour.SpawnDirectly)
                {
                    DirectDamage = true;
                    yield return SpawnDirectly(b);
                }
            }
        }

        IEnumerator RunHoming(AttackVFXBehaviour behavior)
        {
            transform.parent =null;
            yield return null;
            float elapsed = 0f;
            while (elapsed < behavior.ActiveTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator RunNonHoming(AttackVFXBehaviour behavior)
        {
            transform.parent = null;
            yield return null;
            float elapsed = 0f;

            Vector3 target = new Vector3(Target.transform.position.x, Target.transform.position.z, Target.transform.position.z);

            transform.LookAt(target);

            Rigidbody.AddForce((target - transform.position).normalized * behavior.Speed, ForceMode.VelocityChange);

            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                yield return null;
            }

            yield return null;

            while (elapsed < behavior.ActiveTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }

        IEnumerator SpawnDirectly(AttackVFXBehaviour behavior)
        {
            transform.parent = null;
            transform.position = new Vector3(Target.transform.position.x, Target.transform.position.z, Target.transform.position.z);

            if (Owner == VFXOwner.Enemy)
            {
                if (Target.TryGetComponent<PlayerController>(out var p))
                {
                    p.OnHit(Damage);
                }
            }
            else
            {
                if (Target.TryGetComponent<EnemyController>(out var p))
                {
                    p.OnHit(Damage);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public enum VFXOwner
    {
        Player,
        Enemy
    }
}
