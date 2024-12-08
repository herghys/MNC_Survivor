using HerghysStudio.Survivor.Character;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class CharacterData: ScriptableObject
    {
        /// <summary>
        /// Character Name
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// Spawn Position Offset (Reference)
        /// </summary>
        public Vector3 SpawnPosition;

        /// <summary>
        /// Stats
        /// </summary>
        public BaseCharacteStatsData BaseStatsData;

        /// <summary>
        /// Skill Data
        /// </summary>
        public CharacterSkill Skills;
    }
}
