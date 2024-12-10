using System.Collections;

using HerghysStudio.Survivor.Utility.Coroutines;

using UnityEngine;
using UnityEngine.AI;

namespace HerghysStudio.Survivor.Character
{
    public class EnemyMovement : CharacterMovementController
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform target;


        private void LateUpdate()
        {
            if (target == null)
                return;

            if (GameManager.Instance.IsPaused)
            {
                if (!navMeshAgent.isStopped)
                    navMeshAgent.isStopped = true;
            }
            else
            {
                if (navMeshAgent.isOnNavMesh)
                    Move();
            }
        }

        protected internal void Setup(NavMeshAgent agent, Transform target)
        {
            this.target = target;
            navMeshAgent = agent;

            TryMove().Run();
        }

        IEnumerator TryMove()
        {
            if (!navMeshAgent.isOnNavMesh)
                yield return new WaitUntil(() => navMeshAgent.isOnNavMesh == true);
            Move();
        }

        protected internal override void Move()
        {
            navMeshAgent.destination = target.position;
            //navMeshAgent.Move(target.position);
        }
    }
}
