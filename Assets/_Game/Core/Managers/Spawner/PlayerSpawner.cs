using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Utility.Singletons;

using UnityEngine;

namespace HerghysStudio.Survivor.Spawner
{
    public class PlayerSpawner : NonPersistentSingleton<PlayerSpawner>
    {
        [SerializeField] PlayableCharacterData characterData;

        [field: SerializeField] public PlayerController Player { get; private set; }

        /// <summary>
        /// Setup Playable Character Data to be used
        /// </summary>
        /// <param name="_characterData"></param>
        public void SetupPlayerData(PlayableCharacterData _characterData)
        {
            this.characterData = _characterData;
        }

        /// <summary>
        /// Welllllll, Spawn
        /// </summary>
        public PlayerController SpawnPlayer()
        {
            var player = Instantiate(characterData.Prefab);
            this.Player = player;

            Player.SetupData(characterData);
            Player.transform.SetParent(null);

            return Player;
        }
    }
}
