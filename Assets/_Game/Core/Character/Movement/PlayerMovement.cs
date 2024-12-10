using HerghysStudio.Survivor.Inputs;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class PlayerMovement : CharacterMovementController
    {
        Vector3 moveDirection = Vector3.zero;
        private void FixedUpdate()
        {
            Move();
        }

        protected internal override void Move()
        {
            moveDirection = new Vector3(InputManager.Instance.MoveInput.x, 0f, InputManager.Instance.MoveInput.y);
            if (moveDirection != Vector3.zero)
                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.LookRotation(moveDirection), 0.25f);
            rigidBody.MovePosition(rigidBody.position + moveDirection * attributeController.SpeedAttributes.Value * Time.fixedDeltaTime);
            //rigidBody.velocity = moveDirection * attributeController.SpeedAttributes.Value;
        }
    }
}
