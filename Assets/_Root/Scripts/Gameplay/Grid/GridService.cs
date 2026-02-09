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
        private CellsGrid CellsGrid { get; set; }
        
        public event Action OnGridCreated;
        
        public bool MinesGenerated => _minesPositions.Count > 0;
        public Cell[,] Cells => _cells;
        public int MineQuantity => _mineQuantity;
        
        private Cell[,] _cells;
        private int _width;
        private int _height;
        private List<Vector2Int> _minesPositions = new();
        private int _mineQuantity;

        public void CreateGrid(int width, int height, int mineQuantity)
        {
            if (_cells != null)
                DestroyGrid();
            
            _minesPositions.Clear();
            _width = width;
            _height = height;
            _mineQuantity = mineQuantity;
            _cells = CellsGrid.Create(_width, _height);
            OnGridCreated?.Invoke();
        }

        public void ResetGrid()
        {
            if (_cells != null)
                DestroyGrid();
            
            _minesPositions.Clear();
            _cells = CellsGrid.Create(_width, _height);
            OnGridCreated?.Invoke();
        }

        public void GenerateMines(int safeX, int safeY)
        {
            var availablePositions = GetAvailableMinePositions(safeX, safeY);
            Shuffle(availablePositions);
            _mineQuantity = Mathf.Clamp(_mineQuantity, 0, availablePositions.Count);

            for (var i = 0; i < _mineQuantity; i++)
            {
                var position = availablePositions[i];
                var cell = _cells[position.x, position.y];
                cell.SetMineState();
                _minesPositions.Add(position);
            }

            CalculateNumbers();
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
                            
                        var cell = _cells[nx, ny];
                        if(cell.State == CellState.Mine)
                            continue;
                        
                        cell.AddMineToNeighbors();
                    }
                }
            }
        }

        private void DestroyGrid()
        {
            foreach (var cell in _cells)
            {
                cell.Destroy();
            }
            _cells = null;
        }
    }
}