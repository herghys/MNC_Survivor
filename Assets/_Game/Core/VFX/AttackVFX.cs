using System;
using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Utility.Coroutines;
using HerghysStudio.Survivor.Utility.Logger;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.VFX
{
    public abstract class AttackVFX : MonoBehaviour
    {
        private List<AttackVFXBehaviour> _behaviours = new();

        protected AttackVFXBehaviour _currentAttackBehaviour;

        [Header("VFX Component Reference")]
        [SerializeField] protected bool collisionDivideByDuration;
        [SerializeField] protected BoxCollider _boxCollider;
        [SerializeField] protected Rigidbody rigidBody;
        [SerializeField] protected ParticleSystem particle;

        [Header("Outside Reference")]
        [SerializeField] protected VFXOwner Owner;
        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected CharacterSkill skill;
        [SerializeField] protected AttackVFXData data;
        [SerializeField] protected float damage;
        [SerializeField] protected float elapsedActiveTime;
        [SerializeField] protected bool isActivated;

        [field: SerializeField] public bool isHoming { get; protected set; }
        [field: SerializeField] public bool isSpawnOnRandomPosition { get; protected set; }
        [field: SerializeField] public bool isAttackFromOwner { get; protected set; }

        public Vector3 TargetPosition { get; protected set; }
        public Transform Target { get; protected set; }
        public Transform OwnerTransform { get; protected set; }

        public ObjectPool<AttackVFX> Pool { get; protected set; }

        protected virtual void OnEnable()
        {
            GameManager.Instance.OnGameEnded += OnGameEnded;
        }

        protected virtual void OnDisable()
        {
            GameManager.Instance.OnGameEnded -= OnGameEnded;
        }

        protected virtual void OnGameEnded(EndGameState state)
        {
            StopAllCoroutines();
            particle.Stop();
            ReleaseToPool();
        }


        #region Pooling
        public virtual void SetupPool(ObjectPool<AttackVFX> vfxPool)
        {
            Pool = vfxPool;
        }

        protected virtual void ReleaseToPool()
        {
            transform.parent = GameManager.Instance.VFXHolder;
            particle?.Stop();

            try
            {

                Pool.Release(this);
            }
            catch { }
        }
        #endregion

        public virtual void Setup(CharacterSkill _skill, AttackVFXData data, Transform parent, VFXOwner owner, float damage)
        {
            elapsedActiveTime = 0;
            isActivated = false;

            this._currentAttackBehaviour = data.AttackBehaviour;
            this.skill = _skill;
            this.data = data;
            this.layerMask = data.LayerMask;
            this.damage = damage;

            OwnerTransform = parent;
            Owner = owner;

            if (rigidBody == null)
                rigidBody = GetComponent<Rigidbody>();

            if (particle == null)
                particle = GetComponent<ParticleSystem>();

            if (_boxCollider == null)
                _boxCollider = GetComponent<BoxCollider>();
        }

        public virtual void SetupAsSpawned(bool attackFromParent, bool spawnOnRandomPosition)
        {
            isAttackFromOwner = attackFromParent;
            isSpawnOnRandomPosition = spawnOnRandomPosition;
        }

        public virtual void SetupAsProjectile(CharacterSkill _skill, AttackVFXData data, Transform parent, VFXOwner owner, float damage, bool isHoming = false)
        {
            this.isHoming = isHoming;
        }

        public abstract void SetupTarget(Transform target, Vector3 targetPosition, Transform parent, VFXOwner owner);

        public virtual void StartLogic()
        {
            gameObject.SetActive(true);
            transform.parent = null;
            StartCoroutine(StartVFX());
        }

        protected virtual void Activate()
        {
            isActivated = true;
        }

        protected virtual IEnumerator StartVFX()
        {
            yield return new WaitForSeconds(0.25f);


            if (_currentAttackBehaviour.Delay > 0)
                yield return new WaitForSeconds(_currentAttackBehaviour.Delay);

            Activate();

            while (elapsedActiveTime < _currentAttackBehaviour.ActiveTime)
            {
                elapsedActiveTime += Time.deltaTime;
                yield return null;
            }

            ReleaseToPool();
        }



        protected virtual void CheckDamageCollision()
        {
            Vector3 boxCenter = _boxCollider.bounds.center;
            Vector3 boxSize = _boxCollider.bounds.size;

            Collider[] collidersInsideBox = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity);

            foreach (var collider in collidersInsideBox)
            {
                if (collider == null)
                    continue;

                if (Owner == VFXOwner.Enemy)
                {
                    collider.TryGetComponent<PlayerController>(out var target);
                    GameLogger.Log(target);
                    target?.OnHit(Damage());
                }
                else
                {
                    collider.TryGetComponent<EnemyController>(out var target);
                    GameLogger.Log(target);
                    target?.OnHit(Damage());
                }
            }
        }



        protected virtual float Damage()
        {
            return damage;
        }

        //    private void OnEnable()
        //    {
        //        GameManager.Instance.OnGameEnded += OnGameEnded;
        //    }

        //    private void OnDisable()
        //    {
        //        GameManager.Instance.OnGameEnded -= OnGameEnded;

        //    }

        //    private void OnGameEnded(bool arg0)
        //    {
        //        particle.Stop();
        //        try
        //        {
        //            Pool.Release(this);
        //        }
        //        catch { }
        //    }

        //    private void FixedUpdate()
        //    {
        //        if (GameManager.Instance.IsPlayerDead)
        //            return;

        //        if (CurrentBehaviourType == VfxBehaviour.SpawnOnSelf)
        //        {
        //            if (OwnerTransform != null)
        //                transform.position = OwnerTransform.position;
        //        }
        //    }

        //    public void DealDamageOnImpact()
        //    {
        //        if (boxCollider == null)
        //            return;

        //        Debug.Log(name);

        //        Vector3 boxCenter = boxCollider.bounds.center;
        //        Vector3 boxSize = boxCollider.bounds.size;

        //        // Get all colliders inside the BoxCollider using OverlapBox
        //        Collider[] collidersInsideBox = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity);

        //        foreach (var collider in collidersInsideBox)
        //        {
        //            if (collider == null)
        //                continue;

        //            // Deal damage based on the collider's attached component
        //            if (Owner == VFXOwner.Enemy)
        //            {
        //                // Check if the collider is of the player and deal damage
        //                if (collider.TryGetComponent<PlayerController>(out var player))
        //                {
        //                    player.OnHit(Damage);
        //                }
        //            }
        //            else
        //            {
        //                // Check if the collider is of the enemy and deal damage
        //                if (collider.TryGetComponent<EnemyController>(out var enemy))
        //                {
        //                    enemy.OnHit(Damage);
        //                }
        //            }
        //        }
        //    }

        //    IEnumerator StartVFX()
        //    {
        //        DirectDamage = false;
        //        yield return new WaitForSeconds(0.25f);

        //        for (int i = 0; i < Behaviours.Count; i++)
        //        {
        //            var b = Behaviours[i];
        //            CurrentBehaviourType = b.VFXBehaviour;
        //            if (b.VFXBehaviour == VfxBehaviour.Hold)
        //            {
        //                yield return new WaitForSeconds(b.Delay);
        //            }

        //            if (b.VFXBehaviour == VfxBehaviour.MoveToTarget)
        //            {
        //                if (b.IsHoming)
        //                    yield return RunHoming(b);

        //                else
        //                {
        //                    if (Target == null)
        //                        yield return RunNonHomingWithoutTarget(b);
        //                    else
        //                        yield return RunNonHomingWithTarget(b);
        //                }
        //            }

        //            if (b.VFXBehaviour == VfxBehaviour.SpawnDirectly)
        //            {
        //                DirectDamage = true;

        //                yield return SpawnDirectly(b);
        //            }
        //        }
        //    }
        //    public float elapsed = 0f;

        //    IEnumerator RunHoming(AttackVFXBehaviour behavior)
        //    {
        //        yield return null;
        //        elapsed = 0f;
        //        while (elapsed < behavior.ActiveTime)
        //        {
        //            elapsed += Time.deltaTime;
        //            yield return null;
        //        }
        //    }

        //    IEnumerator RunNonHomingWithTarget(AttackVFXBehaviour behavior)
        //    {
        //        yield return null;
        //    }

        //    IEnumerator RunNonHomingWithoutTarget(AttackVFXBehaviour behavior)
        //    {
        //        // Ensure the Rigidbody is assigned
        //        if (Rigidbody == null)
        //        {
        //            Rigidbody = GetComponent<Rigidbody>();
        //        }

        //        // If Rigidbody is still missing, log an error and stop the coroutine
        //        if (Rigidbody == null)
        //        {
        //            Debug.LogError("Rigidbody not found on " + gameObject.name);
        //            yield break;  // Exit if there's no Rigidbody component
        //        }

        //        // Set the initial direction of movement (forward direction of the projectile)
        //        Vector3 moveDirection = OwnerTransform.forward;

        //        // Make sure the Rigidbody doesn't use gravity (unless needed)
        //        Rigidbody.useGravity = false;

        //        // Apply initial velocity in the forward direction
        //        Rigidbody.velocity = moveDirection * behavior.Speed;

        //        // Continuously update the velocity and rotation during the lifetime of the projectile
        //        while (gameObject.activeSelf)
        //        {
        //            // Ensure continuous forward velocity during its lifetime
        //            if (Rigidbody != null)
        //            {
        //                Rigidbody.velocity = moveDirection * behavior.Speed;
        //            }

        //            // Keep the projectile's rotation aligned with its forward direction
        //            transform.rotation = Quaternion.LookRotation(moveDirection);

        //            yield return null;  // Wait for the next frame
        //        }

        //        // Once finished, release the object back to the pool
        //        try
        //        {

        //            Pool.Release(this);
        //        }
        //        catch { }
        //    }

        //    IEnumerator SpawnDirectly(AttackVFXBehaviour behavior)
        //    {
        //        transform.parent = null;
        //        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.z, Target.transform.position.z);

        //        if (Owner == VFXOwner.Enemy)
        //        {
        //            if (Target.TryGetComponent<PlayerController>(out var p))
        //            {
        //                p.OnHit(Damage);
        //            }
        //        }
        //        else
        //        {
        //            if (Target.TryGetComponent<EnemyController>(out var p))
        //            {
        //                p.OnHit(Damage);
        //            }
        //        }

        //        yield return new WaitForSeconds(1f);
        //    }
    }

    public enum VFXOwner
    {
        Player,
        Enemy
    }
}
