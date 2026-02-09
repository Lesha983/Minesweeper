namespace MineSweeper.UI
{
    using Gameplay;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class UIResetButton : MonoBehaviour
    {
        [Inject] 
        private LevelService LevelService { get; set; }
        [Inject]
        private GridService GridService { get; set; }

        [SerializeField]
        private Image buttonImage;
        [SerializeField] 
        private Button button;
        [Space]
        [SerializeField] 
        private Sprite playSprite;
        [SerializeField] 
        private Sprite loseSprite;
        [SerializeField] 
        private Sprite winSprite;

        private void OnEnable()
        {
            button.onClick.AddListener(Reset);
            
            LevelService.OnLevelCompleted += SetWinState;
            LevelService.OnLevelFailed += SetLoseState;
            
            GridService.OnGridCreated += SetGameState;
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Reset);
            
            LevelService.OnLevelCompleted -= SetWinState;
            LevelService.OnLevelFailed -= SetLoseState;
            
            GridService.OnGridCreated -= SetGameState;
        }

        private void Start()
        {
            SetGameState();
        }

        private void Reset()
        {
            GridService.ResetGrid();
        }

        private void SetWinState()
        {
            buttonImage.sprite = winSprite;
        }

        private void SetLoseState()
        {
            buttonImage.sprite = loseSprite;
        }

        private void SetGameState()
        {
            buttonImage.sprite = playSprite;
        }
    }
}