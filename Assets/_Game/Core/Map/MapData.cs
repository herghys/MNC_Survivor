using System.Collections.Generic;

using HerghysStudio.Survivor.Character;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    [CreateAssetMenu(fileName = "MapData", menuName = "Scriptable Objects/MapData")]
    public class MapData : ScriptableObject
    {
        public List<EnemyCharacterData> Enemies;
    }
}
