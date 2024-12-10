using System;
using System.Collections;

using HerghysStudio.Survivor.Utility.Coroutines;

using UnityEditor.Animations;

using UnityEngine;
using UnityEngine.AI;

namespace HerghysStudio.Survivor.Character
{
    public class EnemyMovement : CharacterMovementController
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform target;
        [SerializeField] private EnemyController controller;

        public IEnumerator MoveCoroutine { get; private set; }

        bool isPaused;

        protected override void DoOnAwake()
        {
            controller ??= GetComponent<EnemyController>();
            base.DoOnAwake();
            GameManager.Instance.OnTogglePause += OnTogglePause;
            GameManager.Instance.OnClickedHome += OnClickedHome;
        }

        private void OnClickedHome()
        {
            navMeshAgent.enabled = false;
            StopAllCoroutines();
        }

        private void OnTogglePause(bool pause)
        {
            isPaused = pause;
            if (navMeshAgent.isOnNavMesh)
                navMeshAgent.isStopped = pause;
            if (!gameObject.activeSelf)
                return;

            TryMove().Run();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            GameManager.Instance.OnTogglePause -= OnTogglePause;
            GameManager.Instance.OnClickedHome -= OnClickedHome;

        }

        protected internal void Setup(NavMeshAgent agent, Transform target)
        {
            this.target = target;
            navMeshAgent = agent;

            MoveCoroutine = TryMove();

            navMeshAgent.enabled = true;
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                navMeshAgent.Warp(hit.position); // Place the agent on the nearest valid NavMesh
                navMeshAgent.enabled = true;
            }
            else
            {
                Debug.LogWarning("Enemy not placed on a valid NavMesh!");
            }

            MoveCoroutine.Run();
        }

        IEnumerator TryMove()
        {
            if (!navMeshAgent.isOnNavMesh)
                yield return new WaitUntil(() => navMeshAgent.isOnNavMesh == true);

            if (target == null)
                yield return new WaitUntil(() => target != null);

            while (navMeshAgent != null && !controller.IsDead)
            {
                if (target == null)
                    continue;

                if (controller.IsDead)
                    break;

                navMeshAgent.destination = target.position;

                if (navMeshAgent != null && navMeshAgent.destination != null)
                {
                    while (navMeshAgent != null && (!navMeshAgent.isStopped || navMeshAgent.remainingDistance > 1f))
                    {

                        if (isPaused || GameManager.Instance.IsPlayerDead || controller.IsDead)
                            break;

                        navMeshAgent.destination = target.position;

                        yield return new WaitForSeconds(.05f);
                    }
                }

                if (isPaused || GameManager.Instance.IsPlayerDead)
                    continue;

                yield return new WaitForSeconds(0.05f);
            }
        }

        protected internal override void Move()
        {
            //navMeshAgent.destination = target.position;
            //navMeshAgent.Move(target.position);
        }
    }
}
