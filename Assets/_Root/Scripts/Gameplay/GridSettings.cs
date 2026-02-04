namespace MineSweeper.Gameplay
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "GridSettings", fileName = nameof(GridSettings))]
    public class GridSettings : ScriptableObject
    {
        [field: SerializeField] 
        public int GridWidth { get; private set; }
        [field: SerializeField] 
        public int GridHeight { get; private set; }
        [field: SerializeField] 
        public int MineQuantity { get; private set; }
        [field: Space]
        [field: SerializeField] 
        public GridCell CellPrefab { get; private set; }
        
    }
}