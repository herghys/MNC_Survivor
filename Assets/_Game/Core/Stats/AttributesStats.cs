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
        public float Value;

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
