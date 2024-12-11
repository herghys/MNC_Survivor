using HerghysStudio.Survivor.Inputs;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class PlayerMovement : CharacterMovementController
    {
        Vector3 moveDirection = Vector3.zero;
        [SerializeField] Transform character;
        private void FixedUpdate()
        {
            if (GameManager.Instance.IsPlayerDead) return;
            Move();
        }

        protected internal override void Move()
        {
            moveDirection = new Vector3(InputManager.Instance.MoveInput.x, 0f, InputManager.Instance.MoveInput.y);
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                character.rotation = Quaternion.Slerp(character.rotation, targetRotation, 0.25f);
                //transform.rotation = Quaternion.SlerpUnclamped☺(transform.rotation, Quaternion.LookRotation(moveDirection), 0.25f);

            }
            rigidBody.MovePosition(rigidBody.position + moveDirection * attributeController.SpeedAttributes.Value * Time.fixedDeltaTime);
        }
    }
}
