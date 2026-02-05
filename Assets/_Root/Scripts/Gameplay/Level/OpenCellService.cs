namespace MineSweeper.Gameplay
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class OpenCellService
    {
        [Inject]
        private GridService GridService { get; set; }

        public event Action OnMineCellClicked;
        public event Action OnLevelCompleted;
        
        public void OpenCell(GridCell gridCell)
        {
            if(gridCell.IsOpen || gridCell.IsFlagged)
                return;

            var position = gridCell.GridPosition;
            if (!GridService.MinesGenerated)
                GridService.GenerateMines(position.x, position.y);

            if (gridCell.State == CellState.Mine)
            {
                gridCell.SetExplosiveMine();
                OpenAllMines();
                OnMineCellClicked?.Invoke();
            }
            else if (gridCell.State == CellState.Empty)
                OpenEmptyCells(position.x, position.y);
            else
                gridCell.Open();

            CheckLevelCompleted();
        }

        public void SetFlag(GridCell gridCell)
        {
            gridCell.SetFlag();
            CheckLevelCompleted();
        }

        private void OpenEmptyCells(int startX, int startY)
        {
            var queue = new Queue<Vector2Int>();
            queue.Enqueue(new Vector2Int(startX, startY));
            var grid = GridService.Grid;
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
            var grid = GridService.Grid;
            foreach (var cell in grid)
            {
                if (cell.State == CellState.Mine && !cell.IsFlagged)
                    cell.Open();
            }
        }
        
        private void CheckLevelCompleted()
        {
            var grid = GridService.Grid;
            foreach (var cell in grid)
            {
                if (!cell.IsOpen && !cell.IsFlagged)
                    return;
            }
            
            OnLevelCompleted?.Invoke();
        }
    }
}