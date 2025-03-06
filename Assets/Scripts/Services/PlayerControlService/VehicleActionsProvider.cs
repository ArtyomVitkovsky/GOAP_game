using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.PlayerControlService
{
    public class VehicleActionsProvider : InputSystem_Actions.IVehicleActions
    {
        #region Events

        public Action<Vector2> OnMoveAction;
        public Action<Vector2> OnLookAction;
        public Action OnAttackAction;
        public Action OnExitAction;
        public Action OnHandBreakAction;

        #endregion

        #region Properties
        public Vector2 MoveVector { get; private set; }
        public bool Attack { get; private set; }
        public bool HandBreak { get; private set; }

        #endregion
        
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveVector = context.ReadValue<Vector2>();
            OnMoveAction?.Invoke(MoveVector);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            Attack = context.started || context.performed;
            OnAttackAction?.Invoke();
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            OnExitAction?.Invoke();
        }

        public void OnHandBreak(InputAction.CallbackContext context)
        {
            HandBreak = context.started || context.performed;
        }
    }
}