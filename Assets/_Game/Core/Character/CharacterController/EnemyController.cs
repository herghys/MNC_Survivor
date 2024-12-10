using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Character
{
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyMovement))]
    public class EnemyController : BaseCharacterController<EnemyMovement, EnemyCharacterData>
    {
        /// <summary>
        /// Nav Mesh Agent
        /// </summary>
        [SerializeField] NavMeshAgent navMeshAgent;

        /// <summary>
        /// Player as Target
        /// </summary>
        [SerializeField] private Transform target;

        /// <summary>
        /// Gold Drop on Die
        /// </summary>
        [SerializeField] private GameObject goldDrop;

        /// <summary>
        /// Do on Awake
        /// </summary>
        protected override void DoOnAwake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            base.DoOnAwake();
        }

        /// <summary>
        /// SetupPlayerReference Target
        /// </summary>
        /// <param name="player"></param>
        public void SetupTargetReference(Transform player)
        {
            target = player;
        }

        public IObjectPool<EnemyController> PoolReference;

        /// <summary>
        /// On Character Die
        /// </summary>
        protected override void OnDie()
        {
            //base.OnDie();
            Instantiate(goldDrop, transform.position, Quaternion.identity);
            PoolReference.Release(this);
        }

        /// <summary>
        /// On Get Hit
        /// </summary>
        /// <param name="damage"></param>
        protected override void OnHit(float damage)
        {
            base.OnHit(damage);
        }

        public override void ResetCharacter()
        {
            //throw new System.NotImplementedException();
        }

        #region NavMesh
        /// <summary>
        /// Assigns a new target for the enemy.
        /// </summary>
        public void AssignTarget(Transform newTarget)
        {
            target = newTarget;
        }

        /// <summary>
        /// Stops the NavMeshAgent and disables its movement.
        /// </summary>
        public void StopMovement()
        {
            if (navMeshAgent.enabled)
            {
                navMeshAgent.isStopped = true;
            }
        }

        /// <summary>
        /// Resumes the NavMeshAgent movement.
        /// </summary>
        public void ResumeMovement()
        {
            if (navMeshAgent.enabled)
            {
                navMeshAgent.isStopped = false;
            }
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
