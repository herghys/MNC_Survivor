using System.Collections;

using HerghysStudio.Survivor.Collectables;

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
        /// Face Camera
        /// </summary>
        [SerializeField] private FaceCamera faceCamera;

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
            if (faceCamera != null)
                faceCamera.Camera = GameManager.Instance.MainCamera;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnClickedHome += OnClickedHome;
            GameManager.Instance.OnGameEnded += OnGameEnded;
        }


        private void OnDisable()
        {
            GameManager.Instance.OnGameEnded -= OnGameEnded;
            GameManager.Instance.OnClickedHome -= OnClickedHome;

        }

        private void OnClickedHome()
        {
            if (navMeshAgent.isOnNavMesh)
                navMeshAgent.isStopped = true;
            characterMovement.StopAllCoroutines();
        }

        private void OnGameEnded(bool arg0)
        {
            if (navMeshAgent.isOnNavMesh)
                navMeshAgent.isStopped = true;
            characterMovement.StopAllCoroutines();
        }

        public override void SetupData(EnemyCharacterData characterData)
        {
            base.SetupData(characterData);
            characterAttack.Setup(characterData.BasicAttack, characterData.BasicAttackCount);
        }

        /// <summary>
        /// Add Skill
        /// </summary>
        /// <param name="skill"></param>
        public void AddSkill(CharacterSkill skill)
        {
            SkillSet.Add(skill);
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
            characterAttack.CharacterDied();

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

            void SpawnHealth()
            {
                int randomg = UnityEngine.Random.Range(0, 10);
                if (randomg > 5)
                {
                    var health = CollectableManager.Instance.HealthDropPool.Get();
                    health.transform.position = new Vector3(transform.position.x, health.transform.position.y, transform.position.z);
                    health.Setup(CollectableManager.Instance.GoldDropPool);
                }
            }

            SpawnDrop();
            SpawnHealth();

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
        }

        public void SetupAttribute()
        {
            characterAttribute.SetupAttribute(); 
            characterAttack.StartAttacking();
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
            characterMovement ??= GetComponent<EnemyMovement>();
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
