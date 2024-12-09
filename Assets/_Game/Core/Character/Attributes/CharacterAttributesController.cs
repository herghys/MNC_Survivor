using System;

using HerghysStudio.Survivor.Character;
using HerghysStudio.Survivor.Identifiables;
using HerghysStudio.Survivor.Stats;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class CharacterAttributesController : MonoBehaviour
    {
        private Configurator<AttributeType, AttributesStats> _configurator = new();

        [SerializeField] private BaseCharacteStatsData CharacterDataInstance;

        [field:SerializeField] public HealthAttributes HealthAttributes { get; protected set; } = new();
        [field:SerializeField] public DamageAttributes DamageAttributes { get; protected set; } = new();
        [field:SerializeField] public DefenseAttributes DefenseAttributes { get; protected set; } = new();
        [field:SerializeField] public SpeedAttributes SpeedAttributes { get; protected set; } = new();


        public void SetupAttribute<TCharacterData>(ref TCharacterData characterData) where TCharacterData : CharacterData
        {
            CharacterDataInstance = Instantiate(characterData.BaseStatsData);

            _configurator.Add(CharacterDataInstance.Health.Id, health => health.Value = CharacterDataInstance.Health.Value);

            _configurator.Add(CharacterDataInstance.Attack.Id, damage => damage.Value = CharacterDataInstance.Attack.Value);

            _configurator.Add(CharacterDataInstance.Defense.Id, defense => defense.Value = CharacterDataInstance.Defense.Value);

            _configurator.Add(CharacterDataInstance.Speed.Id, speed => speed.Value = CharacterDataInstance.Defense.Value);

            ReconfigureAll();
        }

        private void OnDisable()
        {
            CharacterDataInstance = null;
        }

        public void AddModifier(AttributeType attribute, float value)
        {
            _configurator.Add(attribute, health => health.Value += value);
            Reconfigure(attribute);
        }

        public void ForceChange(AttributeType attribute, float value)
        {
            _configurator.Add(attribute, health => health.Value = value);
            Reconfigure(attribute);
        }

        public void AddModifierToAny(float value)
        {
            _configurator.Add(ObjectId<AttributeType>.Any, attr => attr.Value += value);
            ReconfigureAll();
        }

        public void ForceChangeToAny(float value)
        {
            _configurator.Add(ObjectId<AttributeType>.Any, attr => attr.Value = value);
            ReconfigureAll();
        }

        private void Reconfigure(AttributeType attribute)
        {
            _configurator.Configure(attribute switch
            {
                AttributeType.Damage => DamageAttributes,
                AttributeType.Defense => DefenseAttributes,
                AttributeType.Speed => SpeedAttributes,
                AttributeType.Health => HealthAttributes,
                _ => (HealthAttributes)
            });
        }

        private void ReconfigureAll(bool withExclusion = false, AttributeType exclusion = AttributeType.Speed)
        {
            foreach (AttributeType attr in Enum.GetValues(typeof(AttributeType)))
            {
                if (withExclusion && attr == exclusion)
                    continue;

                Reconfigure(attr);
            }
        }
    }
}