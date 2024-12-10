using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Character;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Settings")]
        public List<EnemyCharacterData> enemyCharacterData; // List of character data
        public Transform player; // Reference to the player
        public float despawnDistance = 50f; // Distance to despawn enemies

        [Header("Spawn Configuration")]
        public int enemiesPerMinute = 1000; // Enemies to spawn every minute
        public float gameDuration = 180f; // Total game duration in seconds

        [SerializeField] Transform holder;
        private Dictionary<string, ObjectPool<EnemyController>> enemyPools; // Pools for each type of enemy
        private List<EnemyController> activeEnemies = new List<EnemyController>(); // Track active enemies
        [SerializeField] private float spawnCooldown;

        bool IsPlayerDead;

        private void Awake()
        {
            // Calculate cooldown between enemy spawns based on enemies per minute
            spawnCooldown = 60f / enemiesPerMinute;
        }

        private void OnEnable()
        {
            GameManager.Instance.OnPlayerDead += OnPlayerDead;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnPlayerDead -= OnPlayerDead;

        }

        private void OnPlayerDead()
        {
            IsPlayerDead = true;
        }

        public void Setup(List<EnemyCharacterData> enemyData)
        {
            enemyCharacterData = enemyData;
        }

        public void SetupPlayerAndPool(Transform player)
        {
            var _holder = new GameObject("EnemyHolder");
            holder = _holder.transform;

            this.player = player;
            // Initialize enemy pools
            enemyPools = new Dictionary<string, ObjectPool<EnemyController>>();

            foreach (var characterData in enemyCharacterData)
            {
                if (!enemyPools.ContainsKey(characterData.CharacterName))
                {
                    enemyPools.Add(characterData.CharacterName, new ObjectPool<EnemyController>(
                    createFunc: () => CreateEnemy(characterData, enemyPools[characterData.CharacterName]),
                    actionOnGet: enemy => ActivateEnemy(enemy, characterData),
                    actionOnRelease: DeactivateEnemy,
                    actionOnDestroy: DestroyEnemy,
                    collectionCheck: false,
                    defaultCapacity: 10));
                }
            }
        }

        /// <summary>
        /// Start spawning enemies.
        /// </summary>
        public void StartSpawning()
        {
            StartCoroutine(SpawnEnemies());
        }

        /// <summary>
        /// Coroutine to spawn enemies over time.
        /// </summary>
        private IEnumerator SpawnEnemies()
        {
            for (int i = 0; i < enemiesPerMinute; i++)
            {
                if (IsPlayerDead)
                    break;

                if (GameManager.Instance.IsPaused)
                {
                    yield return new WaitUntil(() => GameManager.Instance.IsPaused == false);
                }

                SpawnEnemy();
                yield return new WaitForSeconds(spawnCooldown / 1000f);
            }
        }

        /// <summary>
        /// Spawns a single enemy from the pool.
        /// </summary>
        private void SpawnEnemy()
        {
            if (player == null) return;

            // Get a random character data
            var characterData = enemyCharacterData[Random.Range(0, enemyCharacterData.Count)];

            // Fetch an enemy from the pool
            EnemyController enemy = enemyPools[characterData.CharacterName].Get();

            // Position enemy at a random location around the player
            Vector3 spawnPosition = GetRandomPositionAroundPlayer(20f, 100f);
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;

            activeEnemies.Add(enemy);
        }

        /// <summary>
        /// Gets a random position around the player within a specified range.
        /// </summary>
        private Vector3 GetRandomPositionAroundPlayer(float minDistance, float maxDistance)
        {
            float randomAngle = Random.Range(0f, Mathf.PI * 2);
            float randomDistance = Random.Range(minDistance, maxDistance);

            return player.position + new Vector3(
                Mathf.Cos(randomAngle) * randomDistance,
                0f,
                Mathf.Sin(randomAngle) * randomDistance
            );
        }

        /// <summary>
        /// Create an enemy when needed.
        /// </summary>
        private EnemyController CreateEnemy(EnemyCharacterData data, ObjectPool<EnemyController> pool)
        {
            var enemy = Instantiate(data.Prefab, holder);
            enemy.SetupData(data);
            enemy.gameObject.SetActive(false);
            enemy.PoolReference = pool;
            return enemy;
        }

        /// <summary>
        /// Activates an enemy when fetched from the pool.
        /// </summary>
        private void ActivateEnemy(EnemyController enemy, EnemyCharacterData data)
        {
            enemy.SetupData(data); // Pass character data for initialization
            enemy.SetupTargetReference(player);
            enemy.gameObject.SetActive(true);

            // Notify GameManager about the new active enemy
            GameManager.Instance.ActiveEnemies++;
        }

        /// <summary>
        /// Deactivates an enemy when returned to the pool.
        /// </summary>
        private void DeactivateEnemy(EnemyController enemy)
        {
            activeEnemies.Remove(enemy);
            enemy.ResetCharacter();
            enemy.gameObject.SetActive(false);

            // Notify GameManager about active enemies
            GameManager.Instance.ActiveEnemies--;
        }

        /// <summary>
        /// Destroys an enemy if the pool is cleared.
        /// </summary>
        private void DestroyEnemy(EnemyController enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
