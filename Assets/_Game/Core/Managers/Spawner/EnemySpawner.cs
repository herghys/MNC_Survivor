using System.Collections;
using System.Collections.Generic;

using Codice.Client.BaseCommands.BranchExplorer;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Spawner;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Holder")]
        [SerializeField] private Transform enemiesHolder;
        [SerializeField] private EnemyPoolManager poolPrefab;

        [Header("Enemy Settings")]
        public List<EnemyCharacterData> enemyCharacterData;
        public Transform player;
        public float despawnDistance = 80f;

        [Header("Spawn Configuration")]
        [SerializeField] private int enemiesPerMinute = 1000;
        [SerializeField] private float gameDuration = 180f;
        [SerializeField] private float spawnCooldown;

        private Dictionary<string, EnemyPoolManager> enemyPools;
        private List<EnemyController> activeEnemies = new();

        private void Awake()
        {
            // Initialize cooldown (in seconds) for spawning enemies based on enemiesPerMinute
            spawnCooldown = 60f / enemiesPerMinute;
        }

        /// <summary>
        /// SetupPlayerReference the spawner with a player reference.
        /// </summary>
        public void SetupPlayerReference(Transform player)
        {
            this.player = player;
        }

        /// <summary>
        /// SetupPlayerReference the spawner with enemy character data.
        /// </summary>
        public void Setup(List<EnemyCharacterData> enemyData)
        {
            var holder = new GameObject("Enemies Holder");
            enemiesHolder = holder.GetComponent<Transform>();
            enemiesHolder.parent = null;

            enemyCharacterData = enemyData;

            enemyPools = new();

            foreach (var characterData in enemyCharacterData)
            {
                EnemyPoolManager pool = default;
                if (!enemyPools.ContainsKey(characterData.CharacterName))
                {
                    pool = Instantiate(poolPrefab, transform);
                    enemyPools.Add(characterData.CharacterName, pool);
                }

                pool.SetupContext(this);

                pool.SetupData(
                    data: characterData,
                    holder: enemiesHolder
                    );
            }
        }

        public void ActivatePool()
        {
            Debug.Log("Activate Pool");
            foreach (var pool in enemyPools.Values)
            {
                pool.SetupTarget(this.player);
                pool.ActivatePool();
            }
        }

        /// <summary>
        /// Start spawning enemies over the game duration.
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
            float elapsedTime = 0f;

            while (elapsedTime < GameTimeManager.Instance.elapsedMillisecond)
            {
                SpawnEnemy();
                elapsedTime += spawnCooldown;
                yield return new WaitForSeconds(spawnCooldown);
            }

            Debug.Log("Spawning complete!");
        }

        /// <summary>
        /// Spawns a single enemy at a random position within a certain range of the player.
        /// </summary>
        private void SpawnEnemy()
        {
            if (player == null) return;

            // Get a random character data
            var characterData = enemyCharacterData[Random.Range(0, enemyCharacterData.Count)];

            // Get a random position within range
            Vector3 spawnPosition = GetRandomPositionAroundPlayer(10f, 50f);

            // Fetch an enemy from the pool
            var poolManager = enemyPools[characterData.CharacterName];
            EnemyController enemy = poolManager._pool.Get();

            // Set the enemy's position
            enemy.transform.position = spawnPosition + characterData.SpawnPosition;
            enemy.transform.rotation = Quaternion.identity;

            activeEnemies.Add(enemy);
        }

        /// <summary>
        /// Gets a random position around the player within a specified range.
        /// </summary>
        private Vector3 GetRandomPositionAroundPlayer(float minDistance, float maxDistance)
        {
            // Generate a random angle and distance
            float randomAngle = Random.Range(0f, Mathf.PI * 2);
            float randomDistance = Random.Range(minDistance, maxDistance);

            // Calculate offset in XZ plane
            Vector3 randomOffset = new Vector3(
                Mathf.Cos(randomAngle) * randomDistance,
                0f,
                Mathf.Sin(randomAngle) * randomDistance
            );

            return player.position + randomOffset;
        }

        internal EnemyController CreateEnemy(EnemyController enemyController, Transform holder)
        {
            var enemy = Instantiate(enemyController, holder);
            return enemy;
        }
    }
}
