using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using HerghysStudio.Survivor.Utility.Singletons;

using UnityEngine;

namespace HerghysStudio.Survivor.WorldGeneration
{
    public class WorldGenerator : NonPersistentSingleton<WorldGenerator>
    {
        private GameObject EnvironmentHolder;
        private bool isGenerating = false;
        private Vector3 lastPlayerPosition;


        public Transform Player; // Player's Transform
        public float ChunkSize = 36.3f; // Size of each chunk
        public int RenderDistance = 3; // Number of chunks around the player to render
        public ChunkPoolManager ChunkPoolManager; // Reference to ChunkPoolManager


        private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            if (ChunkPoolManager == null)
                ChunkPoolManager = FindFirstObjectByType<ChunkPoolManager>();

            if (EnvironmentHolder == null)
            {
                EnvironmentHolder = new GameObject("Chunk_Holder");
                EnvironmentHolder.transform.SetParent(null);
                EnvironmentHolder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                EnvironmentHolder.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            }
        }

        private async void Start()
        {
            while (!ChunkPoolManager.IsAwaken)
            {
                await Task.Yield();
            }

            await ChunkPoolManager.InitializePools();

            GenerateInitialChunks();
            RenderDistance += 1;
        }

        void Update()
        {
            if (Player == null)
                return;

            Vector2Int playerChunk = new Vector2Int(
                Mathf.FloorToInt(Player.position.x / ChunkSize),
                Mathf.FloorToInt(Player.position.z / ChunkSize)
            );

            // Load new chunks
            for (int x = -RenderDistance; x <= RenderDistance; x++)
            {
                for (int z = -RenderDistance; z <= RenderDistance; z++)
                {
                    Vector2Int chunkCoord = playerChunk + new Vector2Int(x, z);
                    if (!activeChunks.ContainsKey(chunkCoord))
                    {
                        GenerateChunk(chunkCoord);
                    }
                }
            }

            // Unload distant chunks
            List<Vector2Int> chunksToRemove = new List<Vector2Int>();
            foreach (var chunk in activeChunks)
            {
                if (Vector2Int.Distance(chunk.Key, playerChunk) > RenderDistance)
                {
                    chunksToRemove.Add(chunk.Key);
                }
            }
            foreach (var chunkCoord in chunksToRemove)
            {
                GameObject chunk = activeChunks[chunkCoord];
                activeChunks.Remove(chunkCoord);

                // Return chunk to the pool
                ChunkPoolManager.ReturnChunk(chunk.name.Replace("(Clone)", "").Trim(), chunk);
            }
        }

        private IEnumerator UpdateWorldChunks()
        {
            isGenerating = true;

            // Generate chunks asynchronously or over several frames to avoid frame drops
            for (int x = -RenderDistance; x <= RenderDistance; x++)
            {
                for (int z = -RenderDistance; z <= RenderDistance; z++)
                {
                    Vector2Int chunkCoord = new Vector2Int(x, z);
                    // Ensure you don't generate chunks that are already created
                    if (!activeChunks.ContainsKey(chunkCoord))
                    {
                        // Only generate the chunks we need, and only when the player has moved
                        GenerateChunk(chunkCoord);
                    }
                }

                // Yield return null to spread the chunk generation over multiple frames
                yield return null; // Wait for next frame
            }

            isGenerating = false;
        }

        /// <summary>
        /// Generates the initial chunks centered around (0, 0).
        /// </summary>
        private void GenerateInitialChunks()
        {
            Vector2Int initialChunkCoord = Vector2Int.zero;

            for (int x = -RenderDistance; x <= RenderDistance; x++)
            {
                for (int z = -RenderDistance; z <= RenderDistance; z++)
                {
                    Vector2Int chunkCoord = new Vector2Int(x, z);

                    // Ensure (0, 0) spawns index 0
                    if (chunkCoord == initialChunkCoord)
                    {
                        GenerateChunk(chunkCoord, 0); // Always use index 0 for the center chunk
                    }
                    else
                    {
                        GenerateChunk(chunkCoord);
                    }
                    //Vector2Int chunkCoord = initialChunkCoord + new Vector2Int(x, z);
                    //GenerateChunk(chunkCoord);
                }
            }

            Debug.Log("Initial chunks generated around (0, 0).");
        }
        void GenerateChunk(Vector2Int coord, int? forcedIndex = null)
        {
            Vector3 chunkPosition = new Vector3(coord.x * ChunkSize, 0, coord.y * ChunkSize);

            // Randomly select a chunk type
            //string[] chunkTypes = { "Grass", "Tree", "Water" }; // Names must match pool names
            string selectedChunk = string.Empty;

            if (forcedIndex.HasValue)
            {
                selectedChunk = ChunkPoolManager.GetPoolsId()[forcedIndex.Value];
            }
            else
            {
                selectedChunk = ChunkPoolManager.GetPoolsId()[Random.Range(0, ChunkPoolManager.PoolIdsCount())];
            }

            // Get a chunk from the pool
            GameObject chunk = ChunkPoolManager.GetChunk(selectedChunk);
            chunk.transform.position = chunkPosition;

            chunk.transform.SetParent(EnvironmentHolder.transform);

            // Track active chunks
            activeChunks.Add(coord, chunk);
        }
    }
}
