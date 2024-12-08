using HerghysStudio.Survivor.Character;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    [CreateAssetMenu(fileName = "PlayableCharacterData", menuName = "Data/Character Data/Playable Character")]
    public class PlayableCharacterData : CharacterData
    {
        public PlayerController Prefab;
    }
}
