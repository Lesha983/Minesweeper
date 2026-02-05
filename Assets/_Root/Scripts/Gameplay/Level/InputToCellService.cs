namespace MineSweeper.Gameplay
{
    using Input;
    using UnityEngine;
    using Zenject;

    public class InputToCellService : MonoBehaviour
    {
        [Inject]
        private IInputProvider InputProvider { get; set; }
        [Inject]
        private GridService GridService { get; set; }
        [Inject]
        private OpenCellService OpenCellService { get; set; }
        
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

            OpenCellService.OpenCell(cell);
        }

        private void HandleRightClick(Vector2 pointerPosition)
        {
            if(!TryGetCell(pointerPosition, out var cell))
                return;
            
            OpenCellService.SetFlag(cell);
        }
        
        private bool TryGetCell(Vector2 pointerPosition, out GridCell cell)
        {
            cell = null;
            var worldPos = _camera.ScreenToWorldPoint(pointerPosition);
            var hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (!hit)
                return false;

            cell = hit.collider.GetComponent<GridCell>();
            return true;
        }
    }
}