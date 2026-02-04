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
            // if(!TryGetCell(pointerPosition, out var x, out var y))
            //     return;
            if(!TryGetCell(pointerPosition, out var cell))
                return;

            OpenCellService.OpenCell(cell);
        }

        private void HandleRightClick(Vector2 pointerPosition)
        {
            // if(!TryGetCell(pointerPosition, out var x, out var y))
            //     return;
            if(!TryGetCell(pointerPosition, out var cell))
                return;
            
            OpenCellService.SetFlag(cell);
        }

        // private bool TryGetCell(Vector2 pointerPosition, out int x, out int y)
        // {
        //     var worldPos = _camera.ScreenToWorldPoint(pointerPosition);
        //     var gridOrigin = GridService.CellsParent.position;
        //     var cellSize = GridService.CellSize;
        //     x = Mathf.FloorToInt((worldPos.x - gridOrigin.x)/ cellSize);
        //     y = Mathf.FloorToInt((worldPos.y - gridOrigin.y) / cellSize);
        //     
        //     Debug.LogError($"TryGetCel() - pointerPosition = {pointerPosition}; worldPos = {worldPos}; gridOrigin = {gridOrigin}; cellSize = {cellSize}; x = {x}; y = {y}");
        //     var grid = GridService.Grid;
        //     if(x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1))
        //         return false;
        //     
        //     return true;
        // }
        
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