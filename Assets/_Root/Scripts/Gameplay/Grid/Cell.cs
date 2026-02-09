namespace MineSweeper.Gameplay
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public enum CellState
    {
        Empty,
        Mine,
        Numeric,
    }

    public class Cell : MonoBehaviour
    {
        [SerializeField]
        private Image spriteRenderer;
        [SerializeField] 
        private TMP_Text mineIndicatorLabel;
        [SerializeField]
        private Image mineSprite;
        [SerializeField]
        private Image capSprite;
        [SerializeField]
        private Image flagSprite;
        
        public CellState State => _state;
        public bool IsOpen => _isOpen;
        public bool IsFlagged => _isFlagged;
        public Vector2Int GridPosition => _position;
        
        private CellState _state;
        private bool _isFlagged;
        private bool _isOpen;
        private Vector2Int _position;
        private int _mineCount;

        public void Setup(int x, int y)
        {
            _position = new Vector2Int(x, y);
            _isOpen = false;
            _isFlagged = false;
            _state = CellState.Empty;
            spriteRenderer.color = Color.white;
            _mineCount = 0;
            
            mineIndicatorLabel.gameObject.SetActive(false);
            mineSprite.gameObject.SetActive(false);
            flagSprite.gameObject.SetActive(false);
            
            capSprite.gameObject.SetActive(true);
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

        public void AddMineToNeighbors()
        {
            _state = CellState.Numeric;
            _mineCount++;
            mineIndicatorLabel.text = _mineCount.ToString();
            mineIndicatorLabel.gameObject.SetActive(true);
        }

        public void Open()
        {
            _isOpen = true;
            _isFlagged = false;
            
            flagSprite.gameObject.SetActive(false);
            capSprite.gameObject.SetActive(false);
        }
        
        public void SetFlag()
        {
            if(_isOpen)
                return;
            
            flagSprite.gameObject.SetActive(!_isFlagged);
            _isFlagged = !_isFlagged;
        }

        public void SetExplosiveMine()
        {
            spriteRenderer.color = Color.red;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
