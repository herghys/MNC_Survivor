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
        }

        protected internal abstract void Move();
    }
}