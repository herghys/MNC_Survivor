using UnityEngine;
using UnityEngine.AI;

namespace HerghysStudio.Survivor.Character
{
    public class EnemyController : BaseCharacterController<EnemyMovement, EnemyCharacterData>
    {
        [SerializeField] NavMeshAgent navMeshAgent;

        [SerializeField] private Transform player;

        [SerializeField] private GameObject goldDrop;
        protected override void DoOnAwake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            base.DoOnAwake();
        }

        public void SetupPlayerReference(Transform player)
        {
            this.player = player;
        }

        protected override void OnDie()
        {
            //base.OnDie();
            Instantiate(goldDrop, transform.position, Quaternion.identity);
        }

        protected override void OnHit(float damage)
        {
            base.OnHit(damage);
        }

        public override void ResetCharacter()
        {
            throw new System.NotImplementedException();
        }
    }
}
