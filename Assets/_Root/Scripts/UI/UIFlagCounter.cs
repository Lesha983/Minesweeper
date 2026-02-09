namespace MineSweeper.UI
{
    using Gameplay;
    using UnityEngine;
    using Zenject;

    public class UIFlagCounter : MonoBehaviour
    {
        [Inject] 
        private OpenCellService OpenCellService { get; set; }
        [Inject]
        private GridService GridService { get; set; }
        
        [SerializeField]
        private UINumericSprites numericSprites;

        private void OnEnable()
        {
            OpenCellService.OnFlagsChanged += UpdateFlagsQuantity;
            GridService.OnGridCreated += ResetCounter;
        }

        private void OnDisable()
        {
            OpenCellService.OnFlagsChanged -= UpdateFlagsQuantity;
            GridService.OnGridCreated -= ResetCounter;
        }

        private void Start()
        {
            UpdateFlagsQuantity();
        }

        private void UpdateFlagsQuantity()
        {
            var value = OpenCellService.MineQuantity - OpenCellService.FlagsQuantity;
            numericSprites.SetValue(value);
        }

        private void ResetCounter()
        {
            numericSprites.ResetValue();
            var value = GridService.MineQuantity;
            numericSprites.SetValue(value);;
        }
    }
}