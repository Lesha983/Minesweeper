namespace MineSweeper.Gameplay
{
    using System;
    using UnityEngine;

    [Serializable]
    public class RangeValue
    {
        public int Value;
        public int Min;
        public int Max;
    }
    
    [CreateAssetMenu(menuName = "GridSettings", fileName = nameof(GridSettings))]
    public class GridSettings : ScriptableObject
    {
        [field: SerializeField] 
        public RangeValue StartGridWidth { get; private set; }
        [field: SerializeField] 
        public RangeValue StartGridHeight { get; private set; }
        [field: SerializeField] 
        public int StartMineQuantity { get; private set; }
    }
}