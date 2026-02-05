namespace MineSweeper.UI
{
    using Gameplay;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class UIGameScreen : AUIScreen
    {
        [Inject]
        private LevelService LevelService { get; set; }
        
        [SerializeField]
        private Button startButton;

        private void OnEnable()
        {
            startButton.onClick.AddListener(Restart);
        }
        
        private void OnDisable()
        {
            startButton.onClick.RemoveListener(Restart);
        }
        
        private void Restart()
        {
            LevelService.StartLevel();
        }
    }
}