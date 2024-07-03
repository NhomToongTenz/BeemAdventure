using DuAn1._Project.Scripts.Input_System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DuAn1._Project.Scripts.SampleTest
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Platformer/InputReader")]
    public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2, bool> Look = delegate { };
        public event UnityAction EnableMouseControlCamera = delegate { };
        public event UnityAction DisableMouseControlCamera = delegate { };

        private PlayerInputActions _inputActions;

        public Vector3 Direction => _inputActions.Player.Move.ReadValue<Vector2>();

        private void OnEnable()
        {
            if(_inputActions == null)
            {
                _inputActions = new PlayerInputActions();
                _inputActions.Player.SetCallbacks(this);
            }
        }

        public void EnablePlayerActions() => _inputActions.Enable();

        public void OnMove(InputAction.CallbackContext context){
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }

        bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnFire(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
            }
        }
    }
}