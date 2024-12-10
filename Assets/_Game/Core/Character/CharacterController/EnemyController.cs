using System;
using System.Collections;

using Codice.Client.BaseCommands.BranchExplorer;

using HerghysStudio.Survivor.Collectable;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Character
{
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyMovement))]
    public class EnemyController : BaseCharacterController<EnemyMovement, EnemyCharacterData, EnemyAttack>
    {
        /// <summary>
        /// Nav Mesh Agent
        /// </summary>
        [SerializeField] internal NavMeshAgent navMeshAgent;

        /// <summary>
        /// Player as Target
        /// </summary>
        [SerializeField] private Transform target;

        /// <summary>
        /// Gold Drop on Die
        /// </summary>
        [SerializeField] private GameObject goldDrop;

        /// <summary>
        /// Enemy Pool
        /// </summary>
        public IObjectPool<EnemyController> PoolReference;

        /// <summary>
        /// Do on Awake
        /// </summary>
        protected override void DoOnAwake()
        {
            IsDead = false;
            navMeshAgent ??= GetComponent<NavMeshAgent>();
            base.DoOnAwake();
            GameManager.Instance.OnClickedHome += OnClickedHome;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnClickedHome -= OnClickedHome;

        }

        private void OnClickedHome()
        {
            characterMovement.enabled = false;
            characterMovement.StopAllCoroutines();
        }

        /// <summary>
        /// SetupPlayerReference Target
        /// </summary>
        /// <param name="player"></param>
        public void SetupTargetReference(Transform player)
        {
            target = player;
            IsDead = false;

            //navMeshAgent.enabled = false;
            navMeshAgent.speed = CharacterData.BaseStatsData.Speed.Value;
            navMeshAgent.stoppingDistance = CharacterData.StoppingDistance;
            //navMeshAgent.enabled = true;
            characterMovement.Setup(navMeshAgent, target);
        }

        /// <summary>
        /// On Character Die
        /// </summary>
        protected override void OnDie()
        {
            //base.OnDie();
            IsDead = true;

            StartCoroutine(DieCoroutine());
            
        }

        IEnumerator DieCoroutine()
        {
            characterMovement.StopAllCoroutines();

            yield return new WaitForSeconds(0.25f);

            void SpawnDrop()
            {
                var gold = CollectableManager.Instance.GoldDropPool.Get();
                gold.transform.position = new Vector3(transform.position.x, gold.transform.position.y, transform.position.z);
                gold.Setup(CollectableManager.Instance.GoldDropPool);
            }

            SpawnDrop();

            GameManager.Instance.KilledEnemies++;
            Despawn();
        }

        /// <summary>
        /// On Get Hit
        /// </summary>
        /// <param name="damage"></param>
        public override void OnHit(float damage)
        {
            base.OnHit(damage);
            if (characterAttribute.HealthAttributes.Value <= 0)
                OnDie();
        }

        public override void ResetCharacter()
        {
            //throw new System.NotImplementedException();
        }

        #region NavMesh
        public void Despawn()
        {
            navMeshAgent.enabled = false;
            PoolReference.Release(this);
        }
        #endregion
        protected override void Reset()
        {
            base.Reset();
            navMeshAgent ??= GetComponent<NavMeshAgent>();
            characterMovement ??=  GetComponent<EnemyMovement>();
        }

#if UNITY_EDITOR
        public float damageSample = 90;

        [ContextMenu("Give Damage")]
        public void GiveDamage()
        {
            OnHit(damageSample);
        }
#endif
    }
}
