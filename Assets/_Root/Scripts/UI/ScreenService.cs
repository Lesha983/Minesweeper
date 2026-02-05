namespace MineSweeper.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using Gameplay;
    using UnityEngine;
    using Zenject;

    public class ScreenService : MonoBehaviour
    {
        [Inject]
        private LevelService LevelService { get; set; }
        [Inject]
        private IInstantiator Instantiator { get; set; }
        
        [SerializeField]
        private List<AUIScreen> screens;
        
        private AUIScreen _activeScreen;

        public T SwitchScreenTo<T>() where T : AUIScreen
        {
            var screen = GetScreenByType<T>();
            if(screen.IsShown)
                return screen;
        
            if (_activeScreen != null)
                _activeScreen.Close();
            return CreateScreen(screen);
        }
        
        private void Start()
        {
            ShowGameScreen();
        }
        
        private void OnEnable()
        {
            LevelService.OnLevelStarted += ShowGameScreen;
            LevelService.OnLevelCompleted += ShowWinScreen;
            LevelService.OnLevelFailed += ShowLoseScreen;
        }

        private void OnDisable()
        {
            LevelService.OnLevelStarted -= ShowGameScreen;
            LevelService.OnLevelCompleted -= ShowWinScreen;
            LevelService.OnLevelFailed -= ShowLoseScreen;
        }

        private void ShowWinScreen()
        {
            SwitchScreenTo<UIWinScreen>();
        }

        private void ShowLoseScreen()
        {
            SwitchScreenTo<UILoseScreen>();
        }

        private void ShowGameScreen()
        {
            SwitchScreenTo<UIGameScreen>();
        }
        
        private T CreateScreen<T>(T screen) where T : AUIScreen
        {
            var instance = Instantiator.InstantiatePrefabForComponent<T>(screen, transform);
            instance.Show();
            _activeScreen = instance;
            return instance;
        }
        
        private T GetScreenByType<T>() where T : AUIScreen => 
            (T)screens.FirstOrDefault(x => x.GetType() == typeof(T));
    }
}