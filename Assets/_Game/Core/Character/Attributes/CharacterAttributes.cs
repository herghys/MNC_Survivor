using HerghysStudio.Survivor.Stats;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class CharacterAttributes : MonoBehaviour
    {
        public BaseCharacterData CharacterData;

        public Configurator<AttributeType, AttributesStats> Configurator = new();

        private void Awake()
        {
            Configurator.Add(CharacterData.Health.Id, health => health.Value += CharacterData.Health.Value);

            Configurator.Add(CharacterData.Attack.Id, damage => damage.Value += CharacterData.Attack.Value);

            Configurator.Add(CharacterData.Defense.Id, defense => defense.Value += CharacterData.Defense.Value);

            InitializeModifier();
        }

        private void InitializeModifier()
        {

        }
    }
}
