using System;
using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Utility.Coroutines;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.VFX
{
    public class AttackVFX : MonoBehaviour
    {
        public LayerMask LayerMask;
        public VFXOwner Owner;
        public List<AttackVFXBehaviour> Behaviours = new();
        public Transform Target;
        public Transform OwnerTransform;
        public Vector3 TargetPos;
        public Vector3 MoveDirection;
        public AttackVFXData Data;
        public Rigidbody Rigidbody;

        public float Damage;
        public bool DirectDamage;
        public bool DamagOnnStop;
        public bool DamageOnCollsion;

        public ObjectPool<AttackVFX> Pool;

        VfxBehaviour CurrentBehaviourType;

        [SerializeField] BoxCollider boxCollider;
        [SerializeField] ParticleSystem particle;

        private void OnEnable()
        {
            GameManager.Instance.OnGameEnded += OnGameEnded;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameEnded -= OnGameEnded;

        }

        private void OnGameEnded(bool arg0)
        {
            particle.Stop();
            try
            {
                Pool.Release(this);
            }
            catch { }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (DirectDamage || DamagOnnStop)
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

                try
                {

                    Pool.Release(this);
                }
                catch { }

            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (DirectDamage || DamagOnnStop)
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
                try
                {

                    Pool.Release(this);
                }
                catch { }
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (DamageOnCollsion)
            {
                DealDamageOnImpact();
            }
        }

        private void OnParticleSystemStopped()
        {
            if (DamagOnnStop)
            {
                DealDamageOnImpact();
            }


            if (GameManager.Instance.IsPlayerDead)
                particle.Stop();

            try
            {

                Pool.Release(this);
            }
            catch { }
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsPlayerDead)
                return;

            if (CurrentBehaviourType == VfxBehaviour.SpawnOnSelf)
            {
                if (OwnerTransform != null)
                    transform.position = OwnerTransform.position;
            }
        }

        public void DealDamageOnImpact()
        {
            if (boxCollider == null)
                return;

            Debug.Log(name);

            Vector3 boxCenter = boxCollider.bounds.center;
            Vector3 boxSize = boxCollider.bounds.size;

            // Get all colliders inside the BoxCollider using OverlapBox
            Collider[] collidersInsideBox = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity);

            foreach (var collider in collidersInsideBox)
            {
                if (collider == null)
                    continue;

                // Deal damage based on the collider's attached component
                if (Owner == VFXOwner.Enemy)
                {
                    // Check if the collider is of the player and deal damage
                    if (collider.TryGetComponent<PlayerController>(out var player))
                    {
                        player.OnHit(Damage);
                    }
                }
                else
                {
                    // Check if the collider is of the enemy and deal damage
                    if (collider.TryGetComponent<EnemyController>(out var enemy))
                    {
                        enemy.OnHit(Damage);
                    }
                }
            }
        }

        public void Setup(AttackVFXData data, Transform target, Transform ownerTransform, VFXOwner owner, float damage, bool attackFromSelf = false)
        {
            Data = data;
            LayerMask = data.LayerMask;
            Target = target;
            OwnerTransform = ownerTransform;

            Owner = owner;
            Damage = damage;

            foreach (var item in Data.Behaviours)
            {
                Behaviours.Add(new(item));
            }
            StartVFX().Run();
        }

        public void Setup(AttackVFXData data, Vector3 moveDir, Transform ownerTransform, VFXOwner owner, float damage, bool attackFromSelf = false)
        {
            Data = data;
            LayerMask = data.LayerMask;
            MoveDirection = moveDir;
            OwnerTransform = ownerTransform;

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
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < Behaviours.Count; i++)
            {
                var b = Behaviours[i];
                CurrentBehaviourType = b.VFXBehaviour;
                if (b.VFXBehaviour == VfxBehaviour.Hold)
                {
                    yield return new WaitForSeconds(b.Delay);
                }

                if (b.VFXBehaviour == VfxBehaviour.MoveToTarget)
                {
                    if (b.IsHoming)
                        yield return RunHoming(b);

                    else
                    {
                        if (Target == null)
                            yield return RunNonHomingWithoutTarget(b);
                        else
                            yield return RunNonHomingWithTarget(b);
                    }
                }

                if (b.VFXBehaviour == VfxBehaviour.SpawnDirectly)
                {
                    DirectDamage = true;

                    yield return SpawnDirectly(b);
                }
            }
        }
        public float elapsed = 0f;

        IEnumerator RunHoming(AttackVFXBehaviour behavior)
        {
            yield return null;
            elapsed = 0f;
            while (elapsed < behavior.ActiveTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator RunNonHomingWithTarget(AttackVFXBehaviour behavior)
        {
            yield return null;
        }

        IEnumerator RunNonHomingWithoutTarget(AttackVFXBehaviour behavior)
        {
            // Ensure the Rigidbody is assigned
            if (Rigidbody == null)
            {
                Rigidbody = GetComponent<Rigidbody>();
            }

            // If Rigidbody is still missing, log an error and stop the coroutine
            if (Rigidbody == null)
            {
                Debug.LogError("Rigidbody not found on " + gameObject.name);
                yield break;  // Exit if there's no Rigidbody component
            }

            // Set the initial direction of movement (forward direction of the projectile)
            Vector3 moveDirection = OwnerTransform.forward;

            // Make sure the Rigidbody doesn't use gravity (unless needed)
            Rigidbody.useGravity = false;

            // Apply initial velocity in the forward direction
            Rigidbody.velocity = moveDirection * behavior.Speed;

            // Continuously update the velocity and rotation during the lifetime of the projectile
            while (gameObject.activeSelf)
            {
                // Ensure continuous forward velocity during its lifetime
                if (Rigidbody != null)
                {
                    Rigidbody.velocity = moveDirection * behavior.Speed;
                }

                // Keep the projectile's rotation aligned with its forward direction
                transform.rotation = Quaternion.LookRotation(moveDirection);

                yield return null;  // Wait for the next frame
            }

            // Once finished, release the object back to the pool
            try
            {

                Pool.Release(this);
            }
            catch { }
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
