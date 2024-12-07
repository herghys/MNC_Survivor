using UnityEngine;

using HerghysStudio.Survivor.Utility.Singletons;
using UnityEngine.InputSystem;
using HerghysStudio.Survivor.Utility.Logger;

namespace HerghysStudio.Survivor.Inputs
{
    public class InputManager : NonPersistentSingleton<InputManager>
    {
        /// <summary>
        /// PlayerInput Component References
        /// </summary>
        private PlayerInput playerInput;

        /// <summary>
        /// ["Move"] Input Action
        /// </summary>
        private InputAction _moveAction;

        /// <summary>
        /// Movement Input Values
        /// </summary>
        public Vector2 MoveInput { get; protected set; }


        #region Unity
        /// <summary>
        /// Do On Awake, Inherits from Singleton
        /// </summary>
        public override void DoOnAwake()
        {
            base.DoOnAwake();
            SetupInputs();
            SubscribeEvents();
            InputActionEnabler();
        }
        private void OnDisable()
        {
            InputActionDisabler();
            UnsubscribeEvents();
        }
        private void FixedUpdate()
        {
            GameLogger.Log($"Input: {MoveInput}");
        }
        #endregion

        #region Setup
        /// <summary>
        /// Setup Inputs
        /// </summary>
        private void SetupInputs()
        {
            playerInput = GetComponent<PlayerInput>();

            _moveAction = playerInput.actions["Move"];
        }
        #endregion

        #region Input Action Toggler
        /// <summary>
        /// Enable Input Actions
        /// </summary>
        private void InputActionEnabler()
        {
            _moveAction.Enable();
        }

        /// <summary>
        /// Disable Input Actions
        /// </summary>
        private void InputActionDisabler()
        {
            _moveAction?.Disable();
        }
        #endregion

        #region Events Subs
        /// <summary>
        /// Subscribe Input Actions
        /// </summary>
        private void SubscribeEvents()
        {
            _moveAction.performed += PlayerInput_OnMovePerformed;
            _moveAction.canceled += PlayerInput_OnMoveCanceled;

        }

        /// <summary>
        /// Unsubscribe Input Actions
        /// </summary>
        private void UnsubscribeEvents()
        {
            _moveAction.performed -= PlayerInput_OnMovePerformed;
            _moveAction.canceled -= PlayerInput_OnMoveCanceled;
        }
        #endregion

        #region Move Input Action Callback
        /// <summary>
        /// On Move Performed
        /// </summary>
        /// <param Name="callbackContext"></param>
        public void PlayerInput_OnMovePerformed(InputAction.CallbackContext callbackContext)
        {
            MoveInput = callbackContext.ReadValue<Vector2>();
        }

        /// <summary>
        /// On Move Canceled
        /// </summary>
        /// <param Name="callbackContext"></param>
        public void PlayerInput_OnMoveCanceled(InputAction.CallbackContext callbackContext)
        {
            MoveInput = Vector2.zero;
        }
        #endregion
    }
}