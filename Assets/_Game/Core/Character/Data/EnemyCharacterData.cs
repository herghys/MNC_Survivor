using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    [CreateAssetMenu(fileName = "PlayableCharacterData", menuName = "Data/Character Data/Enemy Character")]

    public class EnemyCharacterData : CharacterData
    {
        /// <summary>
        /// Prefab
        /// </summary>
        public EnemyController Prefab;

        [Header("NavMesh")]
        public float StoppingDistance = 5;
    }
}
