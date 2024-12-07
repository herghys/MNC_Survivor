using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

namespace HerghysStudio.Survivor.WorldGeneration
{
    public class ChunkPoolManager : MonoBehaviour
    {
        [System.Serializable]
        public class ChunkPrefab
        {
            public string Name; // Identifier for the chunk type
            public GameObject Prefab; // Prefab to pool
            public int DefaultCapacity = 10; // Number of preloaded instances
            public int MaxSize = 50; // Maximum size of the pool
        }

        public ChunkPrefab[] ChunkPrefabs; // Prefabs to create pools for
        public bool DebugMode = false; // Enable to log pool usage details

        private Dictionary<string, ObjectPool<GameObject>> pools; // Maps Prefab Name to its pool

        void Awake()
        {
            InitializePools();
        }

        /// <summary>
        /// Initializes object pools for all defined chunk prefabs.
        /// </summary>
        private void InitializePools()
        {
            pools = new Dictionary<string, ObjectPool<GameObject>>();

            foreach (var chunkPrefab in ChunkPrefabs)
            {
                if (chunkPrefab.Prefab == null)
                {
                    Debug.LogError($"Prefab for chunk type '{chunkPrefab.Name}' is missing!");
                    continue;
                }

                // Create a pool for each Prefab
                pools[chunkPrefab.Name] = new ObjectPool<GameObject>(
                    createFunc: () => Instantiate(chunkPrefab.Prefab),
                    actionOnGet: chunk => chunk.SetActive(true),
                    actionOnRelease: chunk => chunk.SetActive(false),
                    actionOnDestroy: Destroy,
                    defaultCapacity: chunkPrefab.DefaultCapacity,
                    maxSize: chunkPrefab.MaxSize
                );

                if (DebugMode)
                    Debug.Log($"Initialized pool for '{chunkPrefab.Name}' with capacity {chunkPrefab.DefaultCapacity}.");
            }
        }

        /// <summary>
        /// Retrieves a chunk from the pool by its Name.
        /// </summary>
        /// <param Name="chunkName">The Name of the chunk type.</param>
        /// <returns>A pooled chunk GameObject.</returns>
        public GameObject GetChunk(string chunkName)
        {
            if (!pools.ContainsKey(chunkName))
            {
                Debug.LogError($"No pool found for chunk type '{chunkName}'!");
                return null;
            }

            var chunk = pools[chunkName].Get();

            if (DebugMode)
                Debug.Log($"Retrieved chunk of type '{chunkName}' from pool.");

            return chunk;
        }

        /// <summary>
        /// Returns a chunk back to its pool.
        /// </summary>
        /// <param Name="chunkName">The Name of the chunk type.</param>
        /// <param Name="chunk">The GameObject to return.</param>
        public void ReturnChunk(string chunkName, GameObject chunk)
        {
            if (!pools.ContainsKey(chunkName))
            {
                Debug.LogError($"No pool found for chunk type '{chunkName}'!");
                Destroy(chunk); // Safeguard against memory leaks
                return;
            }

            pools[chunkName].Release(chunk);

            if (DebugMode)
                Debug.Log($"Returned chunk of type '{chunkName}' to pool.");
        }
    }
}
