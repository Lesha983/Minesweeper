namespace MineSweeper.Input
{
    using System;
    using UnityEngine;

    public interface IInputProvider
    {
        event Action<Vector2> OnLeftClick;
        event Action<Vector2> OnRightClick;
        
        void Enable();
        void Disable();
    }
}