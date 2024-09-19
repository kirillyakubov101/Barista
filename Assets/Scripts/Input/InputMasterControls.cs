using MyUtils;
using System;
using UnityEngine.InputSystem;

namespace Barista.Input
{
    public class InputMasterControls : Singleton<InputMasterControls>
    {
        public event Action OnPrimaryAction;
        public event Action<float> OnHorizontalMove;

        private MasterConrols m_masterControls;

        private void Awake()
        {
            m_masterControls = new MasterConrols();
        }

        private void OnEnable()
        {
            m_masterControls.Enable();
            m_masterControls.Default.Action.performed += OnAction;
            m_masterControls.Default.Action.Enable();

            m_masterControls.Default.CameraHorizontalMove.Enable();
            m_masterControls.Default.CameraHorizontalMove.performed += OnCameraHorizontalMove;

        }

        private void OnDisable()
        {
            m_masterControls.Disable();
            m_masterControls.Default.Action.performed -= OnAction;
            m_masterControls.Default.Action.Disable();

            m_masterControls.Default.CameraHorizontalMove.Disable();
        }

        public void OnAction(InputAction.CallbackContext context)
        {
            OnPrimaryAction?.Invoke();
        }

        private void OnCameraHorizontalMove(InputAction.CallbackContext context)
        {
            OnHorizontalMove?.Invoke(context.ReadValue<float>());
        }
    }
}
