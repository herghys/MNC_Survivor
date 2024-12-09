using System.Collections.Generic;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public abstract class BaseCharacterController<TMovementController, TCharacterData> : MonoBehaviour 
        where TMovementController : CharacterMovementController
        where TCharacterData : CharacterData
    {
        /// <summary>
        /// Character Data
        /// </summary>
        [SerializeField] public TCharacterData CharacterData;

        /// <summary>
        /// Attribute Controller
        /// </summary>
        [SerializeField] protected CharacterAttributesController characterAttribute;

        /// <summary>
        /// Character Movement
        /// </summary>
        [SerializeField] protected TMovementController characterMovement;

        /// <summary>
        /// Character Animator
        /// </summary>
        [SerializeField] protected CharacterAnimatorController characterAnimator;

        /// <summary>
        /// Character Health
        /// </summary>
        [SerializeField] protected CharacterHealthController characterHealth;

        public List<CharacterSkill> SkillSet { get; protected set; }

        protected virtual void Awake()
        {
            characterAttribute??=GetComponent<CharacterAttributesController>();
            characterMovement ??= GetComponent<TMovementController>();
            characterAnimator ??= GetComponent<CharacterAnimatorController>();
            characterHealth ??= GetComponent<CharacterHealthController>();

            DoOnAwake();
        }

        protected virtual void DoOnAwake()
        {

        }

        public virtual void SetupData(TCharacterData characterData)
        {
            this.CharacterData = characterData;
            characterAttribute.SetupAttribute(ref CharacterData);
        }

        protected abstract void OnDie();

        protected virtual void OnHit(float damage)
        {
            characterAttribute.HealthAttributes.Value -= damage;
        }

        public abstract void ResetCharacter();
    }
}
