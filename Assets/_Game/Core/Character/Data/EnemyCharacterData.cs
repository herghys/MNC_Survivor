using HerghysStudio.Survivor.Character;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    [CreateAssetMenu(fileName = "PlayableCharacterData", menuName = "Data/Character Data/Enemy Character")]

    public class EnemyCharacterData : CharacterData
    {
        public EnemyController Prefab;

    }
}
