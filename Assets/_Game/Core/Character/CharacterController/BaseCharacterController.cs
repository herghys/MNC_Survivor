using System.Collections.Generic;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    [RequireComponent(typeof(CharacterAttributesController), typeof(CharacterAnimatorController), typeof(CharacterHealthController))]
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

        /// <summary>
        /// Additional On Awake
        /// </summary>
        protected virtual void DoOnAwake() { }

        /// <summary>
        /// SetupPlayerReference Initialize Data
        /// </summary>
        /// <param name="characterData"></param>
        public virtual void SetupData(TCharacterData characterData)
        {
            this.CharacterData = characterData;
            characterAttribute.SetupAttribute(ref CharacterData);
        }

        /// <summary>
        /// On Character Die
        /// </summary>
        protected abstract void OnDie();

        /// <summary>
        /// On Character Get Hit
        /// </summary>
        /// <param name="damage"></param>
        protected virtual void OnHit(float damage)
        {
            characterAttribute.HealthAttributes.Value -= damage;
        }

        /// <summary>
        /// Reset Character
        /// </summary>
        public abstract void ResetCharacter();

        protected virtual void Reset()
        {
            characterAttribute ??= GetComponent<CharacterAttributesController>();
            characterAnimator ??= GetComponent<CharacterAnimatorController>();
            characterHealth ??= GetComponent<CharacterHealthController>();
        }
    }
}
