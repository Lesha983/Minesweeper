namespace MineSweeper.Gameplay
{
    using System;
    using Input;
    using UnityEngine;
    using Zenject;

    public interface IInputToCell
    {
        event Action<Cell> OnTryOpenCell;
        event Action<Cell> OnTrySetFlag;
    }

    public class InputToCellService : MonoBehaviour, IInputToCell
    {
        [Inject]
        private IInputProvider InputProvider { get; set; }
        
        public event Action<Cell> OnTryOpenCell;
        public event Action<Cell> OnTrySetFlag;
        
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }
        
        private void OnEnable()
        {
            InputProvider.OnLeftClick += HandleLeftClick;
            InputProvider.OnRightClick += HandleRightClick;
        }
        
        private void OnDisable()
        {
            InputProvider.OnLeftClick -= HandleLeftClick;
            InputProvider.OnRightClick -= HandleRightClick;
        }
        
        private void HandleLeftClick(Vector2 pointerPosition)
        {
            if(!TryGetCell(pointerPosition, out var cell))
                return;

            OnTryOpenCell?.Invoke(cell);
        }

        private void HandleRightClick(Vector2 pointerPosition)
        {
            if(!TryGetCell(pointerPosition, out var cell))
                return;
            
            OnTrySetFlag?.Invoke(cell);
        }
        
        private bool TryGetCell(Vector2 pointerPosition, out Cell cell)
        {
            cell = null;
            var worldPos = _camera.ScreenToWorldPoint(pointerPosition);
            var hit = Physics2D.Raycast(worldPos, Vector2.zero);
        
            if (!hit)
                return false;
        
            // Debug.LogError($"Trying to find cell {worldPos}; hit.position = {hit.transform.position}");
            cell = hit.collider.GetComponent<Cell>();
            return true;
        }
        
        // private bool TryGetCell(Vector2 pointerPosition, out Cell cell)
        // {
        //     cell = null;
        //     var worldPos = _camera.ScreenToWorldPoint(pointerPosition);
        //     var cells = GridService.Cells;
        //     foreach (var gridCell in cells)
        //     {
        //         // if (!gridCell.transform.position.Equals(worldPos))
        //         //     continue;
        //         
        //         cell = gridCell;
        //         return true;
        //     }
        //     return false;
        // }
    }
}