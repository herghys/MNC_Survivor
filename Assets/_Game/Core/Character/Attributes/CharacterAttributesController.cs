using System;
using System.Collections.Generic;

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

        public void SetupAttribute<TCharacterData>(TCharacterData characterData, bool autoSetup = false) where TCharacterData : CharacterData
        {
            CharacterDataInstance = Instantiate(characterData.BaseStatsData);
            if (autoSetup)
                SetupAttribute();
        }

        public void SetupAttribute()
        {
            _configurator.Add(CharacterDataInstance.Health.Id, health => health.Value = CharacterDataInstance.Health.Value);

            _configurator.Add(CharacterDataInstance.Attack.Id, damage => damage.Value = CharacterDataInstance.Attack.Value);

            _configurator.Add(CharacterDataInstance.Defense.Id, defense => defense.Value = CharacterDataInstance.Defense.Value);

            _configurator.Add(CharacterDataInstance.Speed.Id, speed => speed.Value = CharacterDataInstance.Defense.Value);

            ReconfigureAll();

            HealthAttributes.SetValueAsMax();
            DamageAttributes.SetValueAsMax();
            DefenseAttributes.SetValueAsMax();
            SpeedAttributes.SetValueAsMax();

            _configurator.ClearConfig(ObjectId<AttributeType>.Any);
        }

        public void ResetAttributes()
        {
            _configurator = new();
            HealthAttributes = new();
            DamageAttributes = new();
            DefenseAttributes = new();
            SpeedAttributes = new();

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

        public void AddMaxModifier(AttributeType attribute, float value)
        {
            AttributesStats selectedAttribute = attribute switch
            {
                AttributeType.Damage => DamageAttributes,
                AttributeType.Defense => DefenseAttributes,
                AttributeType.Speed => SpeedAttributes,
                AttributeType.Health => HealthAttributes,
                _ => (HealthAttributes)
            };

            selectedAttribute.MaxValue += value;
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


#if UNITY_EDITOR
        [ContextMenu("Add Sample Config")]
        public void AddSampleHealthMaxConfig()
        {
            AddMaxModifier(AttributeType.Health, 20);
        }
#endif
    }
}
