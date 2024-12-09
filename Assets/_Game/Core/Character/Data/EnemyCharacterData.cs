using HerghysStudio.Survivor.Character;

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

    }
}
