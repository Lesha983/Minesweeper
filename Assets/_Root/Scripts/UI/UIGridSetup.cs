namespace MineSweeper.UI
{
    using Gameplay;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class UIGridSetup : MonoBehaviour
    {
        [Inject] 
        private GridSettings GridSettings { get; set; }
        [Inject]
        private GridService GridService { get; set; }

        [SerializeField] 
        private Button updateButton;
        [Space]
        [SerializeField]
        private TMP_InputField widthField;
        [SerializeField]
        private TMP_InputField heightField;
        [SerializeField]
        private TMP_InputField minesField;
        
        private int _width;
        private int _height;
        private int _mineQuantity;

        private int _minWidth;
        private int _maxWidth;
        private int _minHeight;
        private int _maxHeight;

        private void OnEnable()
        {
            updateButton.onClick.AddListener(UpdateGrid);
            
            widthField.onEndEdit.AddListener(Apply);
            heightField.onEndEdit.AddListener(Apply);
            minesField.onEndEdit.AddListener(Apply);
        }

        private void OnDisable()
        {
            updateButton.onClick.RemoveListener(UpdateGrid);
            
            widthField.onEndEdit.RemoveListener(Apply);
            heightField.onEndEdit.RemoveListener(Apply);
            minesField.onEndEdit.RemoveListener(Apply);
        }

        private void Start()
        {
            _width = GridSettings.StartGridWidth.Value;
            _height = GridSettings.StartGridHeight.Value;
            _mineQuantity = GridSettings.StartMineQuantity;

            _minWidth = GridSettings.StartGridWidth.Min;
            _maxWidth = GridSettings.StartGridWidth.Max;
            _minHeight = GridSettings.StartGridHeight.Min;
            _maxHeight = GridSettings.StartGridHeight.Max;
            
            widthField.text = _width.ToString();
            heightField.text = _height.ToString();
            minesField.text = _mineQuantity.ToString();
            
            UpdateGrid();
        }
        
        private void Apply(string _)
        {
            _width  = ParseInput(widthField,  _minWidth, _maxWidth);
            _height = ParseInput(heightField, _minHeight, _maxHeight);
            
            var maxMines = _width * _height - 1;
            _mineQuantity = ParseInput(minesField, 1, maxMines);
        }

        private int ParseInput(TMP_InputField input, int min, int max)
        {
            if (!int.TryParse(input.text, out int value))
                value = min;

            value = Mathf.Clamp(value, min, max);
            input.text = value.ToString();

            return value;
        }
        
        private void UpdateGrid()
        {
            GridService.CreateGrid(_width, _height, _mineQuantity);
        }
    }
}