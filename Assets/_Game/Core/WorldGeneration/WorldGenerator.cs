using System.Collections.Generic;

using UnityEngine;

namespace HerghysStudio.Survivor.WorldGeneration
{
    public class WorldGenerator : MonoBehaviour
    {
        public Transform Player; // Player's Transform
        public int ChunkSize = 50; // Size of each chunk
        public int RenderDistance = 3; // Number of chunks around the player to render
        public ChunkPoolManager ChunkPoolManager; // Reference to ChunkPoolManager

        private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();

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

        void GenerateChunk(Vector2Int coord)
        {
            Vector3 chunkPosition = new Vector3(coord.x * ChunkSize, 0, coord.y * ChunkSize);

            // Randomly select a chunk type
            string[] chunkTypes = { "Grass", "Tree", "Water" }; // Names must match pool names
            string selectedChunk = chunkTypes[Random.Range(0, chunkTypes.Length)];

            // Get a chunk from the pool
            GameObject chunk = ChunkPoolManager.GetChunk(selectedChunk);
            chunk.transform.position = chunkPosition;

            // Track active chunks
            activeChunks.Add(coord, chunk);
        }
    }
}
