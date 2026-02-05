namespace MineSweeper.Gameplay
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "GridSettings", fileName = nameof(GridSettings))]
    public class GridSettings : ScriptableObject
    {
        [field: Range(2, 15)]
        [field: SerializeField] 
        public int GridWidth { get; private set; }
        [field: Range(2, 15)]
        [field: SerializeField] 
        public int GridHeight { get; private set; }
        [field: SerializeField] 
        public int MineQuantity { get; private set; }
        [field: Space]
        [field: SerializeField] 
        public GridCell CellPrefab { get; private set; }

        public void OnValidate()
        {
            MineQuantity = Mathf.Clamp(MineQuantity, 0, GridWidth * GridHeight - 1);
        }
    }
}