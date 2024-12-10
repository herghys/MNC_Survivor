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
        }

        /// <summary>
        /// SetupPlayerReference Target
        /// </summary>
        /// <param name="player"></param>
        public void SetupTargetReference(Transform player)
        {
            target = player;

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
            Instantiate(goldDrop, transform.position, Quaternion.identity);
            Despawn();
        }

        /// <summary>
        /// On Get Hit
        /// </summary>
        /// <param name="damage"></param>
        public override void OnHit(float damage)
        {
            base.OnHit(damage);
        }

        public override void ResetCharacter()
        {
            //throw new System.NotImplementedException();
        }

        #region NavMesh
        public void Despawn()
        {
            PoolReference.Release(this);
        }
        #endregion
        protected override void Reset()
        {
            base.Reset();
            navMeshAgent ??= GetComponent<NavMeshAgent>();
            characterMovement ??=  GetComponent<EnemyMovement>();
        }
    }
}
