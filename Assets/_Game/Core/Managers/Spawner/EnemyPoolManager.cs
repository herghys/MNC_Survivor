using HerghysStudio.Survivor.Character;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Spawner
{
    public class EnemyPoolManager : MonoBehaviour
    {
        internal IObjectPool<EnemyController> _pool;
        [field:SerializeField] public EnemyCharacterData Data { get; private set; }
        [field:SerializeField] public Transform Holder { get; private set; }
        [field:SerializeField] public Transform Target { get; private set; }
        
        [field:SerializeField] public EnemySpawner Spawner { get; private set; }

        public void SetupContext(EnemySpawner spawner)
        {
            this.Spawner = spawner;
        }

        public void SetupData(EnemyCharacterData data, Transform holder)
        {
            this.Data = data;
            this.Holder = holder;
        }

        public void SetupTarget(Transform target)
        {
            this.Target = target;
        }

        private void Start()
        {
            
        }

        public void ActivatePool()
        {
            if (Data.Prefab == null)
            {
                Debug.Log("NUL");
            }

            _pool = new ObjectPool<EnemyController>(CreatePooledEnemy, GetPooledEnemy, ReleasePooledEnemy, DestroyPooledEnemy);
        }

        private EnemyController CreatePooledEnemy()
        {
            var enemyGameObject = Instantiate(Data.Prefab.gameObject);

            var enemy = enemyGameObject.GetComponent<EnemyController>();
            enemy.PoolReference = _pool;
            enemy.SetupData(Data);
            Debug.Log($"Enemy: {enemy}");
            enemy.gameObject.SetActive(false);
            return enemy;
        }



        /// <summary>
        /// Activates an enemy when fetched from the pool.
        /// </summary>
        private void GetPooledEnemy(EnemyController enemy)
        {
            enemy.gameObject.SetActive(true);
            enemy.SetupData(Data);
            enemy.SetupTargetReference(Target);
        }

        /// <summary>
        /// Deactivates an enemy when returned to the pool.
        /// </summary>
        private void ReleasePooledEnemy(EnemyController enemy)
        {
            //activeEnemies.Remove(enemy);
            enemy.ResetCharacter();
            enemy.gameObject.SetActive(false);

            // Notify GameManager about active enemy
            //GameManager.Instance?.UpdateActiveEnemyCount(activeEnemies.Count);
        }

        /// <summary>
        /// Destroys an enemy if needed.
        /// </summary>
        private void DestroyPooledEnemy(EnemyController enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
