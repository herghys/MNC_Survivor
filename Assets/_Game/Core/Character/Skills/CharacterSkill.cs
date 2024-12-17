using HerghysStudio.Survivor.VFX;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    [CreateAssetMenu(fileName = "CharacterSkill", menuName = "Skill/Character Skill")]

    public class CharacterSkill : ScriptableObject
    {
        public AttackTargetType AttackTargetType;
        public SkillEffectType EffectType;
        //public SkillObjectEffect SkillObjectEffect = new();
        public AttackVFXData AttackVFXData;
        public float ProjectileSpeed = 1;
        public float cooldown;

        [Range(1, 50)]
        public float minRandomSpawnRange;
        [Range(1, 50)]
        public float maxRandomSpawnRange;
    }
    
    public class SkillObjectEffect
    {
        public bool ShowEffect;
        public GameObject EffectPrefab;
    }

    public enum SkillEffectType
    {
        Damage,
        Protection,
        Buff
    }

    public enum AttackTargetType
    {
        HomingProjectile,
        ProjectileToPosition,
        SpawnOnRandomPosition,
        SpawnOnOpponentTarget,
        SpawnOnSelf
    }
}
