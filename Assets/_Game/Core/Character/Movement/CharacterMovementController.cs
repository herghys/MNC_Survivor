using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public abstract class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] protected Rigidbody rigidBody;
        [SerializeField] protected CharacterAttributesController attributeController;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            attributeController = GetComponent<CharacterAttributesController>();
            DoOnAwake();
        }

        protected virtual void DoOnAwake() { }
        protected internal abstract void Move();
    }
}