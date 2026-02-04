namespace MineSweeper.Input
{
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Zenject;

    public class InputProvider : IInitializable, IInputProvider
    {
        public event Action<Vector2> OnLeftClick;
        public event Action<Vector2> OnRightClick;
        
        public Vector2 PointerPosition => Pointer.current.position.ReadValue();
        public InputSystem_Actions InputAction { get; private set; }
        
        public void Initialize()
        {
            InputAction = new InputSystem_Actions();
            BindEvents();
            Enable();
        }

        public void Enable()
        {
            InputAction.Enable();
        }
        
        public void Disable()
        {
            InputAction.Disable();
        }

        private void BindEvents()
        {
            var actions = InputAction.UI;
            actions.Click.performed += ctx => OnLeftClick?.Invoke(PointerPosition);
            actions.RightClick.performed += ctx => OnRightClick?.Invoke(PointerPosition);
        }
    }
}