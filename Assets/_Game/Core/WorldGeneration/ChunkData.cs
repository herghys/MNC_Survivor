using UnityEngine;

namespace HerghysStudio.Survivor
{
    [CreateAssetMenu(fileName = "ChunkData", menuName = "Data/Environments/Chunk")]
    public class ChunkData : ScriptableObject, Identifiables.Identifiable<string>
    {
        [field: SerializeField] public string Id { get; set; }

        public GameObject Prefab; // Prefab to pool
        public int DefaultCapacity = 10; // Number of preloaded instances
        public int MaxSize = 50; // Maximum size of the pool

    }
}
