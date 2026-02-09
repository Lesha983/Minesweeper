namespace MineSweeper.Gameplay
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class OpenCellService : IInitializable, IDisposable
    {
        [Inject]
        private GridService GridService { get; set; }
        [Inject]
        private IInputToCell InputToCell { get; set; }

        public event Action OnMineCellClicked;
        public event Action OnLevelCompleted;
        public event Action OnFlagsChanged;
        public event Action OnFirstCellClicked;
        
        public int MineQuantity => GridService.MineQuantity;
        public int FlagsQuantity => _flagsQuantity;

        private int _flagsQuantity;
        private bool _levelStarted;

        public void Initialize()
        {
            GridService.OnGridCreated += Reset;
            InputToCell.OnTryOpenCell += OpenCell;
            InputToCell.OnTrySetFlag += SetFlag;
        }
        
        public void Dispose()
        {
            GridService.OnGridCreated -= Reset;
            InputToCell.OnTryOpenCell -= OpenCell;
            InputToCell.OnTrySetFlag -= SetFlag;
        }

        private void Reset()
        {
            _flagsQuantity = 0;
            _levelStarted = false;
            OnFlagsChanged?.Invoke();
        }

        private void OpenCell(Cell cell)
        {
            if(cell.IsOpen || cell.IsFlagged)
                return;

            var position = cell.GridPosition;
            if (!GridService.MinesGenerated)
                GridService.GenerateMines(position.x, position.y);

            if (cell.State == CellState.Mine)
            {
                cell.SetExplosiveMine();
                OpenAllMines();
                OnMineCellClicked?.Invoke();
                return;
            }
            else if (cell.State == CellState.Empty)
                OpenEmptyCells(position.x, position.y);
            else
                cell.Open();

            if(!_levelStarted)
                OnFirstClicked();
            
            CheckLevelState();
        }

        private void SetFlag(Cell cell)
        {
            if(cell.IsFlagged)
                _flagsQuantity--;
            else if(_flagsQuantity >= MineQuantity)
                return;
            else
                _flagsQuantity++;

            if (!_levelStarted)
                OnFirstClicked();
            
            cell.SetFlag();
            CheckLevelState();
            OnFlagsChanged?.Invoke();
        }

        private void OnFirstClicked()
        {
            _levelStarted = true;
            OnFirstCellClicked?.Invoke();
        }

        private void OpenEmptyCells(int startX, int startY)
        {
            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            var grid = GridService.Cells;
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);

            while (queue.Count > 0)
            {
                var pos = queue.Dequeue();
                var x = pos.x;
                var y = pos.y;

                var cell = grid[x, y];

                if (cell.IsOpen)
                    continue;
                
                cell.Open();
                
                if (cell.State == CellState.Numeric)
                    continue;
                
                for (var dx = -1; dx <= 1; dx++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;

                        var nx = x + dx;
                        var ny = y + dy;

                        if (nx < 0 || nx >= width || ny < 0 || ny >= height)
                            continue;

                        var neighbor = grid[nx, ny];

                        if (!neighbor.IsOpen && neighbor.State != CellState.Mine)
                        {
                            queue.Enqueue(new Vector2Int(nx, ny));
                        }
                    }
                }
            }
        }

        private void OpenAllMines()
        {
            var grid = GridService.Cells;
            foreach (var cell in grid)
            {
                if (cell.State == CellState.Mine && !cell.IsFlagged)
                    cell.Open();
            }
        }
        
        private void CheckLevelState()
        {
            var grid = GridService.Cells;
            foreach (var cell in grid)
            {
                if (!cell.IsOpen && !cell.IsFlagged)
                    return;
            }
            
            OnLevelCompleted?.Invoke();
        }
    }
}