using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Character;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Settings")]
        public List<EnemyCharacterData> enemyCharacterData; // List of character data
        public Transform player; // Reference to the player
        public Transform[] spawnPoints; // Spawn points for enemies
        public float despawnDistance = 50f; // Distance to despawn enemies

        [Header("Spawn Configuration")]
        public int enemiesPerMinute = 1000; // Enemies to spawn every minute
        public float gameDuration = 180f; // Total game duration in seconds

        private Dictionary<string, ObjectPool<EnemyController>> enemyPools; // Pools for each type of enemy
        private List<EnemyController> activeEnemies = new List<EnemyController>(); // Track active enemies
        private float spawnCooldown;

        public void Setup(Transform player)
        {
            this.player = player;
        }

        public void Setup(List<EnemyCharacterData> enemyData)
        {
            enemyCharacterData = enemyData;
            enemyPools = new Dictionary<string, ObjectPool<EnemyController>>();

            foreach (var characterData in enemyCharacterData)
            {
                enemyPools[characterData.CharacterName] = new ObjectPool<EnemyController>(
                    createFunc: () => Instantiate(characterData.Prefab),
                    actionOnGet: enemy => ActivateEnemy(enemy, characterData),
                    actionOnRelease: DeactivateEnemy,
                    actionOnDestroy: DestroyEnemy,
                    collectionCheck: false,
                    defaultCapacity: 100
                );
            }
        }

        /// <summary>
        /// Coroutine to spawn enemies over time.
        /// </summary>
        private IEnumerator SpawnEnemies()
        {
            float elapsedTime = 0f;

            while (elapsedTime < gameDuration)
            {
                for (int i = 0; i < enemiesPerMinute; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(spawnCooldown);
                }

                elapsedTime += 60f;
            }

            Debug.Log("Spawning complete!");
        }

        /// <summary>
        /// Spawns a single enemy at a random spawn point.
        /// </summary>
        private void SpawnEnemy()
        {
            // Get a random character data
            var characterData = enemyCharacterData[Random.Range(0, enemyCharacterData.Count)];

            // Get a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Get an enemy from the pool
            EnemyController enemy = enemyPools[characterData.CharacterName].Get();

            // Set enemy position with offset
            enemy.transform.position = spawnPoint.position + characterData.SpawnPosition;
            enemy.transform.rotation = spawnPoint.rotation;

            activeEnemies.Add(enemy);
        }

        /// <summary>
        /// Returns an enemy to the pool.
        /// </summary>
        /// <param name="enemy">Enemy to return.</param>
        private void ReturnEnemy(EnemyController enemy)
        {
            activeEnemies.Remove(enemy);
            enemyPools[enemy.CharacterData.CharacterName].Release(enemy);
        }

        /// <summary>
        /// Activates an enemy when fetched from the pool.
        /// </summary>
        private void ActivateEnemy(EnemyController enemy, EnemyCharacterData data)
        {
            enemy.gameObject.SetActive(true);
            enemy.SetupData( data); // Pass CharacterData for initialization
            enemy.SetupPlayerReference(player);
        }

        /// <summary>
        /// Deactivates an enemy when returned to the pool.
        /// </summary>
        private void DeactivateEnemy(EnemyController enemy)
        {
            enemy.ResetCharacter();
            enemy.gameObject.SetActive(false);
        }

        /// <summary>
        /// Destroys an enemy if needed.
        /// </summary>
        private void DestroyEnemy(EnemyController enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
