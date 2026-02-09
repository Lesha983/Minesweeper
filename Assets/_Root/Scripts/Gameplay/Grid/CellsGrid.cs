namespace MineSweeper.Gameplay
{
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class CellsGrid : MonoBehaviour
    {
        [Inject]
        private IInstantiator Instantiator { get; set; }
        
        [SerializeField]
        private Transform cellsParent;
        [SerializeField]
        private GridLayoutGroup grid;
        [SerializeField]
        private Cell cellPrefab;
        
        private Cell[,] _cells;

        public Cell[,] Create(int width, int height)
        {
            grid.constraintCount = width;
            var cells = new Cell[width, height];
            
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var cell = Instantiator.InstantiatePrefabForComponent<Cell>(cellPrefab, cellsParent);
                    cell.Setup(x, y);
                    cells[x, y] = cell;
                }
            }
            return cells;
        }
    }
}