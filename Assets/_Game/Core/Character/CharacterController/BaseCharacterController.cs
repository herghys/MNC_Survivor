using System.Collections.Generic;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    [RequireComponent(typeof(CharacterAttributesController), typeof(CharacterAnimatorController), typeof(CharacterHealthController))]
    public abstract class BaseCharacterController<TMovementController, TCharacterData, TCharacterAttack> : MonoBehaviour 
        where TMovementController : CharacterMovementController
        where TCharacterData : CharacterData
        where TCharacterAttack : CharacterAttack
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

        /// <summary>
        /// CharacterAttack
        /// </summary>
        [SerializeField] protected TCharacterAttack characterAttack;

        public bool IsDead { get; protected set; }
        public List<CharacterSkill> SkillSet { get; protected set; }


        protected virtual void Awake()
        {
            characterAttribute??=GetComponent<CharacterAttributesController>();
            characterMovement ??= GetComponent<TMovementController>();
            characterAttack ??= GetComponent<TCharacterAttack>();
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
            characterAttribute.SetupAttribute(CharacterData, true);
        }

        /// <summary>
        /// On Character Die
        /// </summary>
        protected abstract void OnDie();

        /// <summary>
        /// On Character Get Hit
        /// </summary>
        /// <param name="damage"></param>
        public virtual void OnHit(float damage)
        {
            characterAttribute.HealthAttributes.Value -= damage;
            characterHealth.UpdateHealth(characterAttribute.HealthAttributes.Value / characterAttribute.HealthAttributes.MaxValue, characterAttribute.HealthAttributes.Value);
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
