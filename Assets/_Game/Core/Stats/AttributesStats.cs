using System;

using HerghysStudio.Survivor.Identifiables;
using HerghysStudio.Survivor.Utility;

using UnityEngine;

namespace HerghysStudio.Survivor.Stats
{
    public class AttributesStats : Identifiable<AttributeType>
    {
        [field: SerializeReference]
        public virtual AttributeType Id{ get; set; }

        [field: SerializeReference]
        private float _value;
        public float Value
        {
            get
            {
                if (_value <= 0)
                    return 0;
                return _value;
            }
            set
            {
                _value = value;
                if (_value <= 0)
                    _value = 0;
            }
        }

        [field: SerializeField]public float MaxValue { get; set; }

        public void SetValueAsMax()
        {
            MaxValue = Value;
        }

        public void AddMax(float value)
        {
            bool updateValue = Value == MaxValue;

            MaxValue += value;
            if (updateValue)
                Value = MaxValue;
        }
    }

    [Serializable]
    public class HealthAttributes : AttributesStats
    {
        public HealthAttributes()
        {
            Id = AttributeType.Health;
        }
    }

    [Serializable]
    public class DamageAttributes : AttributesStats
    {
        public DamageAttributes()
        {
            Id = AttributeType.Damage;
        }
    }

    [Serializable]
    public class DefenseAttributes : AttributesStats
    {
        public DefenseAttributes()
        {
            Id = AttributeType.Defense;
        }
    }

    [Serializable]
    public class SpeedAttributes : AttributesStats
    {
        public SpeedAttributes()
        {
            Id = AttributeType.Speed;
        }
    }


    public enum AttributeType
    {
        Health,
        Damage,
        Defense,
        Speed
    }
}
