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
        [SerializeField] protected TCharacterData characterData;

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

        private void Awake()
        {
            characterAttribute??=GetComponent<CharacterAttributesController>();
            characterMovement ??= GetComponent<TMovementController>();
            characterAnimator ??= GetComponent<CharacterAnimatorController>();
            characterHealth ??= GetComponent<CharacterHealthController>();

            Initialize();
        }


        protected virtual void Initialize()
        {
            characterAttribute.SetupAttribute(ref characterData);
        }

        protected abstract void OnDie();

        protected abstract void OnHit();
    }
}
