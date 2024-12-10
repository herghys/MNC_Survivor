using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using HerghysStudio.Survivor.Utility.Singletons;

using UnityEngine;

namespace HerghysStudio.Survivor.WorldGeneration
{
    public class WorldGenerator : NonPersistentSingleton<WorldGenerator>
    {
        private ChunkHolder EnvironmentHolder;
        private bool isGenerating = false;
        private Vector3 lastPlayerPosition;
        private float chunkMoveThreshold = 12.1f;


        public Transform Player; // Player's Transform
        public float ChunkSize = 36.3f; // Size of each chunk
        public int RenderDistance = 3; // Number of chunks around the player to render
        public ChunkPoolManager ChunkPoolManager; // Reference to ChunkPoolManager


        private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            ChunkPoolManager ??= FindFirstObjectByType<ChunkPoolManager>();


            if (EnvironmentHolder == null)
            {
                EnvironmentHolder = Instantiate(ChunkPoolManager.chunkHolderPrefab);
                EnvironmentHolder.transform.SetParent(null);
                EnvironmentHolder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }

        public void SetPlayer(Transform player)
        {
            Player = player;
            lastPlayerPosition = player.position;
        }

        private async void Start()
        {
            while (!ChunkPoolManager.IsAwaken)
            {
                await Task.Yield();
            }

            await ChunkPoolManager.InitializePools();

            GenerateInitialChunks();
        }

        void Update()
        {
            if (Player == null)
                return;

            // Calculate squared distance to avoid using expensive Vector3.Distance()
            float sqrDistance = (Player.position - lastPlayerPosition).sqrMagnitude;
            if (sqrDistance >= chunkMoveThreshold * chunkMoveThreshold)
            {
                lastPlayerPosition = Player.position;
                //StartCoroutine(UpdateWorldChunks());
                UpdateWorldChunks();
            }


        }

        List<Task> generationTasks = new();
        private async void UpdateWorldChunks()
        {
            isGenerating = true;

            Vector2Int playerChunk = new Vector2Int(
                Mathf.FloorToInt(Player.position.x / ChunkSize),
                Mathf.FloorToInt(Player.position.z / ChunkSize)
            );

            // Load new chunks asynchronously
            List<Vector2Int> newChunks = new List<Vector2Int>();
            for (int x = -RenderDistance; x <= RenderDistance; x++)
            {
                for (int z = -RenderDistance; z <= RenderDistance; z++)
                {
                    Vector2Int chunkCoord = playerChunk + new Vector2Int(x, z);
                    if (!activeChunks.ContainsKey(chunkCoord))
                    {
                        newChunks.Add(chunkCoord);
                        generationTasks.Add(GenerateChunkAsync(chunkCoord));
                    }
                }
            }

            // Wait for all chunk generation tasks to complete
            await Task.WhenAll(generationTasks);

            // Unload distant chunks
            await UnloadDistantChunks(playerChunk);

            EnvironmentHolder?.RebakeNavMesh();

            isGenerating = false;
        }

        private async Task GenerateChunkAsync(Vector2Int coord)
        {
            Vector3 chunkPosition = new Vector3(coord.x * ChunkSize, 0, coord.y * ChunkSize);

            // Randomly select a chunk type from the pool
            string selectedChunk = ChunkPoolManager.GetPoolsId()[Random.Range(0, ChunkPoolManager.PoolIdsCount())];

            // Get a chunk from the pool
            GameObject chunk = ChunkPoolManager.GetChunk(selectedChunk);
            chunk.transform.position = chunkPosition;

            chunk.transform.SetParent(EnvironmentHolder.transform);

            // Track active chunks
            activeChunks.Add(coord, chunk);

            await Task.Yield(); // Yield to prevent blocking the main thread
        }


        private async Task UnloadDistantChunks(Vector2Int playerChunk)
        {
            // Collect chunks to remove
            List<Vector2Int> chunksToRemove = new List<Vector2Int>();

            // Identify chunks that are too far away
            foreach (var chunk in activeChunks)
            {
                if (Vector2Int.Distance(chunk.Key, playerChunk) > RenderDistance + 1) // Buffer zone of 1 chunk
                {
                    chunksToRemove.Add(chunk.Key);
                }
            }

            // Unload chunks after iteration is complete
            List<Task> unloadTasks = new List<Task>();
            foreach (var chunkCoord in chunksToRemove)
            {
                unloadTasks.Add(UnloadChunkAsync(chunkCoord));
            }

            // Wait for all chunk unloading tasks to complete
            await Task.WhenAll(unloadTasks);
        }

        /// <summary>
        /// Unloads
        /// </summary>
        /// <param name="chunkCoord"></param>
        /// <returns></returns>
        private async Task UnloadChunkAsync(Vector2Int chunkCoord)
        {
            // Remove chunk from the active collection
            GameObject chunk = activeChunks[chunkCoord];
            activeChunks.Remove(chunkCoord);

            // Return chunk to the pool
            ChunkPoolManager.ReturnChunk(chunk.name.Replace("(Clone)", "").Trim(), chunk);

            await Task.Yield(); // Yield to prevent blocking the main thread
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

            EnvironmentHolder?.RebakeNavMesh();
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

#if UNITY_EDITOR
        [ContextMenu("Update Player")]
        public void UpdatePlayer()
        {
            if (Player != null)
            {
                lastPlayerPosition = Player.position;
            }
        }
#endif
    }
}
