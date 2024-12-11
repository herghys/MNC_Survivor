using HerghysStudio.Survivor.VFX;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    [CreateAssetMenu(fileName = "CharacterSkill", menuName = "Skill/Character Skill")]

    public class CharacterSkill : ScriptableObject
    {
        public bool IsHoming;
        public AttackTargetType AttackTargetType;
        public SkillEffectType EffectType;
        //public SkillObjectEffect SkillObjectEffect = new();
        public AttackVFXData AttackVFXData;
        public float cooldown;

        [Range(1, 50)]
        public float minRandomSpawnRange;
        [Range(1, 50)]
        public float maxRandomSpawnRange;

        //public GameObject SpawnObjectEffect(Transform position, Transform parent)
        //{
        //    var effect = Instantiate(SkillObjectEffect.EffectPrefab, position, parent);
        //    return effect;
        //}
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
        Target,
        RandomPosition,
        CharacterRotation,
        OnSelf
    }
}
