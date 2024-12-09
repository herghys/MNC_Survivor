using System;

using HerghysStudio.Survivor.WorldGeneration;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class PlayerController : BaseCharacterController<PlayerMovement, PlayableCharacterData>
    {
        public override void SetupData(PlayableCharacterData characterData)
        {
            base.SetupData(characterData);
        }

        public void AddSkill(CharacterSkill skill)
        {
            SkillSet.Add(skill);
        }
        protected override void OnDie()
        {
            GameManager.Instance.OnPlayerDead();
        }

        protected override void OnHit(float damage)
        {
            base.OnHit(damage);

            if (characterAttribute.HealthAttributes.Value <= 0)
            {
                OnDie();
            }
        }

        public override void ResetCharacter()
        {
            throw new System.NotImplementedException();
        }

#if UNITY_EDITOR
        public float damageSample = 5;

        [ContextMenu("Give Damage")]
        public void GiveDamage()
        {
            OnHit(damageSample);
        }
#endif
    }
}
