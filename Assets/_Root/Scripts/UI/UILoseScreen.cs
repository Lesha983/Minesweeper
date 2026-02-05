namespace MineSweeper.UI
{
    using Gameplay;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class UILoseScreen : AUIScreen
    {
        [Inject]
        private LevelService LevelService { get; set; }
        
        [SerializeField]
        private Button restartButton;

        private void OnEnable()
        {
            restartButton.onClick.AddListener(Restart);
        }
        
        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(Restart);
        }
        
        private void Restart()
        {
            LevelService.StartLevel();
        }
    }
}