using System.Collections;
using System.Collections.Generic;

using Unity.AI.Navigation;

using UnityEngine;

namespace HerghysStudio.Survivor.WorldGeneration
{
        [RequireComponent(typeof(NavMeshSurface))]
    public class ChunkHolder : MonoBehaviour
    {
        [SerializeField] NavMeshSurface navMeshSurface;
        private void Awake()
        {
            navMeshSurface ??= GetComponent<NavMeshSurface>();
        }

        public void RebakeNavMesh()
        {
            navMeshSurface.BuildNavMesh();
            //
        }

        private void Reset()
        {
            navMeshSurface ??= GetComponent<NavMeshSurface>();
        }
    }
}
