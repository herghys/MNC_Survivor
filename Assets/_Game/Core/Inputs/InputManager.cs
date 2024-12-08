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
        private InputSystem_Actions _inputSystemActions;

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

        private void Update()
        {
            ReadMoveInput();
        }
        #endregion

        #region Setup
        /// <summary>
        /// Setup Inputs
        /// </summary>
        private void SetupInputs()
        {
            _inputSystemActions = new();

            _moveAction = _inputSystemActions.Player.Move;
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
            

        }

        /// <summary>
        /// Unsubscribe Input Actions
        /// </summary>
        private void UnsubscribeEvents()
        {
            
        }
        #endregion

        #region Move Input Action Callback
        private void ReadMoveInput()
        {
            MoveInput = _moveAction.ReadValue<Vector2>();
        }
        #endregion
    }
}