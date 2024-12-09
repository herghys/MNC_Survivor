using HerghysStudio.Survivor.Inputs;

using UnityEngine;

namespace HerghysStudio.Survivor.Character
{
    public class PlayerMovement : CharacterMovementController
    {
        private void FixedUpdate()
        {
            Move();
        }

        protected override void Move()
        {
            Vector3 moveDirection = new Vector3(InputManager.Instance.MoveInput.x, 0f, InputManager.Instance.MoveInput.y);
            rigidBody.MovePosition(rigidBody.position + moveDirection * attributeController.SpeedAttributes.Value * Time.fixedDeltaTime);
            //rigidBody.velocity = moveDirection * attributeController.SpeedAttributes.Value;
        }
    }
}
