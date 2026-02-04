namespace MineSweeper.Gameplay
{
    using System;
    using TMPro;
    using UnityEngine;

    public enum CellState
    {
        Empty,
        Mine,
        Numeric,
    }
    
    public class GridCell : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshPro mineIndicatorLabel;
        [SerializeField]
        private SpriteRenderer mineSprite;
        [SerializeField]
        private SpriteRenderer capSprite;
        [SerializeField]
        private SpriteRenderer flagSprite;
        
        public CellState State => _state;
        
        private CellState _state;
        private bool _isFlagged;
        private Vector2Int _position;

        public void Setup(int x, int y)
        {
            _position = new Vector2Int(x, y);
        }
        
        public void SetMineState()
        {
            _state = CellState.Mine;
            mineSprite.gameObject.SetActive(true);
        }
        
        public void SetNumericState(int value)
        {
            _state = CellState.Numeric;
            mineIndicatorLabel.text = value.ToString();
            mineIndicatorLabel.gameObject.SetActive(true);
        }

        private void Awake()
        {
            mineIndicatorLabel.gameObject.SetActive(false);
            mineSprite.gameObject.SetActive(false);
            flagSprite.gameObject.SetActive(false);
            
            capSprite.gameObject.SetActive(true);
        }
    }
}
