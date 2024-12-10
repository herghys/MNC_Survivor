using HerghysStudio.Survivor.VFX;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    [CreateAssetMenu(fileName = "CharacterSkill", menuName = "Skill/Character Skill")]

    public class CharacterSkill : ScriptableObject
    {
        public SkillEffectType EffectType;
        public SkillObjectEffect SkillObjectEffect = new();
        public AttackVFXData AttackVFXData;
        public float cooldown;

        public GameObject SpawnObjectEffect(Transform position, Transform parent)
        {
            var effect = Instantiate(SkillObjectEffect.EffectPrefab, position, parent);
            return effect;
        }
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
}
