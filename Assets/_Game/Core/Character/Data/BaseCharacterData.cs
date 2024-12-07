using HerghysStudio.Survivor.Stats;

using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

namespace HerghysStudio.Survivor
{
    [CreateAssetMenu(fileName = "BaseCharacterData", menuName = "Data/Character")]
    public class BaseCharacterData : ScriptableObject
    {
        public string CharacterName;

        public HealthAttributes Health = new();
        public DamageAttributes Attack = new();
        public DefenseAttributes Defense = new();
        public SpeedAttributes Speed = new();


        private void Reset()
        {
            Health.Id = AttributeType.Health;
            Attack.Id = AttributeType.Damage;
            Defense.Id = AttributeType.Defense;
            Speed.Id = AttributeType.Speed;
        }
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (Health.Id != AttributeType.Health)
                Health.Id = AttributeType.Health;

            if (Attack.Id != AttributeType.Damage)
                Attack.Id = AttributeType.Damage;

            if (Defense.Id != AttributeType.Defense)
                Defense.Id = AttributeType.Defense;

            if (Speed.Id != AttributeType.Speed)
                Speed.Id = AttributeType.Speed;
        }
#endif
    }

}