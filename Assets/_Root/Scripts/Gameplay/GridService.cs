namespace MineSweeper.Gameplay
{
    using System;
    using UnityEngine;
    using Zenject;
    using Random = UnityEngine.Random;

    public class GridService : MonoBehaviour
    {
        [Inject] 
        private GridSettings GridSettings { get; set; }
        [Inject]
        private IInstantiator Instantiator { get; set; }
        
        [SerializeField]
        private Transform cellsParent;
        
        public bool MinesGenerated => _minesGenerated;
        
        private GridCell[,] _grid;
        private int _width;
        private int _height;
        private bool _minesGenerated;

        public void CreateGrid()
        {
            if (_grid != null)
                DestroyGrid();
            
            _width = GridSettings.GridWidth;
            _height = GridSettings.GridHeight;
            var prefab = GridSettings.CellPrefab;

            _grid = new GridCell[_width, _height];
            
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var cell = Instantiator.InstantiatePrefabForComponent<GridCell>(prefab, new Vector3(x,y,0), Quaternion.identity, cellsParent);
                    cell.Setup(x, y);
                    _grid[x, y] = cell;
                }
            }
        }

        public void GenerateMines(int safeX, int safeY)
        {
            var placedMines = 0;
            var targetMines = GridSettings.MineQuantity;

            while (placedMines < targetMines)
            {
                var x = Random.Range(0, _width);
                var y = Random.Range(0, _height);
                
                if(x == safeX && y == safeY)
                    continue;

                var cell = _grid[x,y];
                if(cell.State == CellState.Mine)
                    continue;
                
                cell.SetMineState();
                placedMines++;
            }

            CalculateNumbers();
            _minesGenerated = true;
        }

        private void Awake()
        {
            CreateGrid();
        }

        private void CalculateNumbers()
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var gridCell = _grid[x, y];
                    if (gridCell.State == CellState.Mine)
                        continue;

                    var count = 0;

                    for (var dx = -1; dx <= 1; dx++)
                    {
                        for (var dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0)
                                continue;
                            
                            var nx = x + dx;
                            var ny = y + dy;
                            if(nx < 0 || ny < 0 || nx >= _width || ny >= _height)
                                continue;
                            
                            var cell = _grid[x + dx, y + dy];
                            if (cell.State == CellState.Mine)
                                count++;
                        }
                    }
                    
                    gridCell.SetNumericState(count);
                }
            }
        }

        private void DestroyGrid()
        {
            foreach (var cell in _grid)
            {
                Destroy(cell.gameObject);
            }
            _grid = null;
        }
    }
}