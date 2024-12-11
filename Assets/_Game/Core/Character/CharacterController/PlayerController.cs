using System;

using HerghysStudio.Survivor.WorldGeneration;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class PlayerController : BaseCharacterController<PlayerMovement, PlayableCharacterData, PlayerAttack>
    {
        protected override void DoOnAwake()
        {
            base.DoOnAwake();
            GameManager.Instance.OnTimerEnded += OnTimerEnded;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnTimerEnded -= OnTimerEnded;

        }
        /// <summary>
        /// On Timer ended
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnTimerEnded()
        {
            if (characterAttribute.HealthAttributes.Value > 0)
            {
                GameManager.Instance.WinGame();
            }
            else
            {
                GameManager.Instance.PlayerDead();
            }
        }

        /// <summary>
        /// Setup Character Data
        /// </summary>
        /// <param name="characterData"></param>
        public override void SetupData(PlayableCharacterData characterData)
        {
            IsDead = false;
            base.SetupData(characterData);
        }

        /// <summary>
        /// Add Skill
        /// </summary>
        /// <param name="skill"></param>
        public void AddSkill(CharacterSkill skill)
        {
            SkillSet.Add(skill);
        }

        /// <summary>
        /// On Character Dead
        /// </summary>
        protected override void OnDie()
        {
            IsDead = true;
            GameManager.Instance.PlayerDead();
        }

        /// <summary>
        /// On Get Hit
        /// </summary>
        /// <param name="damage"></param>
        public override void OnHit(float damage)
        {
            base.OnHit(damage);

            if (characterAttribute.HealthAttributes.Value <= 0)
            {
                OnDie();
            }
        }

        /// <summary>
        /// On Receive Health
        /// </summary>
        /// <param name="health"></param>
        public virtual void OnReceiveHealth(float health)
        {
            characterAttribute.HealthAttributes.Value += health;
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
