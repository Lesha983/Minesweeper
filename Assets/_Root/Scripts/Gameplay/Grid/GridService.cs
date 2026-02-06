namespace MineSweeper.Gameplay
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;
    using Random = UnityEngine.Random;

    public class GridService
    {
        [Inject] 
        private GridSettings GridSettings { get; set; }
        [Inject]
        private CellsSpawner CellsSpawner { get; set; }
        
        public bool MinesGenerated => _minesGenerated;
        public GridCell[,] Grid => _grid;
        
        private GridCell[,] _grid;
        private int _width;
        private int _height;
        private bool _minesGenerated;
        private List<Vector2Int> _minesPositions = new();

        public void CreateGrid()
        {
            if (_grid != null)
                DestroyGrid();
            
            _minesGenerated = false;
            _width = GridSettings.GridWidth;
            _height = GridSettings.GridHeight;
            _grid = CellsSpawner.CreateGrid(_width, _height);
        }

        public void GenerateMines(int safeX, int safeY)
        {
            var availablePositions = GetAvailableMinePositions(safeX, safeY);
            Shuffle(availablePositions);
            var targetMines = GridSettings.MineQuantity;
            targetMines = Mathf.Clamp(targetMines, 0, availablePositions.Count);

            for (var i = 0; i < targetMines; i++)
            {
                var position = availablePositions[i];
                var cell = _grid[position.x, position.y];
                cell.SetMineState();
                _minesPositions.Add(position);
            }

            CalculateNumbers();
            _minesGenerated = true;
        }

        private List<Vector2Int> GetAvailableMinePositions(int safeX, int safeY)
        {
            var result = new List<Vector2Int>();
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    if (x == safeX && y == safeY)
                        continue;
                    
                    result.Add(new Vector2Int(x, y));
                }
            }
            
            return result;
        }
        
        private void Shuffle<T>(List<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private void CalculateNumbers()
        {
            foreach (var minesPosition in _minesPositions)
            {
                for (var dx = -1; dx <= 1; dx++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;
                            
                        var nx = minesPosition.x + dx;
                        var ny = minesPosition.y + dy;
                        if(nx < 0 || ny < 0 || nx >= _width || ny >= _height)
                            continue;
                            
                        var cell = _grid[nx, ny];
                        if(cell.State == CellState.Mine)
                            continue;
                        
                        cell.AddMineToNeighbors();
                    }
                }
            }
        }

        private void DestroyGrid()
        {
            foreach (var cell in _grid)
            {
                cell.Destroy();
            }
            _grid = null;
        }
    }
}