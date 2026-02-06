namespace MineSweeper.Gameplay
{
    using UnityEngine;
    using Zenject;

    public class CellsSpawner : MonoBehaviour
    {
        [Inject]
        private IInstantiator Instantiator { get; set; }
        
        [SerializeField]
        private Transform cellsParent;
        [SerializeField]
        private GridCell cellPrefab;

        public GridCell[,] CreateGrid(int width, int height)
        {
            var cellSize = cellPrefab.transform.localScale.x;
            var grid = new GridCell[width, height];
            
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var cell = Instantiator.InstantiatePrefabForComponent<GridCell>(cellPrefab, cellsParent);
                    var targetPosition = new Vector2(x * cellSize, y * cellSize);
                    cell.transform.position = targetPosition;
                    cell.Setup(x, y);
                    grid[x, y] = cell;
                }
            }
            
            var offsetX = width * cellSize * 0.5f;
            var offsetY = height * cellSize * 0.5f;
            var center = cellsParent.position;
            center -= new Vector3(offsetX, offsetY, 0);;
            cellsParent.position = center;
            return grid;
        }
    }
}