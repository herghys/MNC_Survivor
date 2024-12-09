using UnityEngine;

namespace HerghysStudio.Survivor
{
    [CreateAssetMenu(fileName = "CharacterSkill", menuName = "Skill/Character Skill")]

    public class CharacterSkill : ScriptableObject
    {
        public SkillEffectType EffecType;
        public SkillObjectEffect SkillObjectEffect = new();
        public SkillEffectApplication SkillEffectApplication = new();

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

    public class SkillEffectApplication
    {
        public GameObject EffectApplicationPrefab;
    }

    public enum SkillEffectType
    {
        Damage,
        Protection,
        Buff
    }
}
