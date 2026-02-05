namespace MineSweeper.Gameplay
{
    using System;
    using Input;
    using Zenject;

    public class LevelService : IInitializable, IDisposable
    {
        [Inject]
        private GridService GridService { get; set; }
        [Inject]
        private IInputProvider InputProvider { get; set; }
        [Inject]
        private OpenCellService OpenCellService { get; set; }
        
        public event Action OnLevelCompleted;
        public event Action OnLevelFailed;
        public event Action OnLevelStarted;
        
        public void Initialize()
        {
            OpenCellService.OnMineCellClicked += Lose;
            OpenCellService.OnLevelCompleted += Win;
        }

        public void Dispose()
        {
            OpenCellService.OnMineCellClicked -= Lose;
            OpenCellService.OnLevelCompleted -= Win;
        }

        public void StartLevel()
        {
            InputProvider.Enable();
            GridService.CreateGrid();
            OnLevelStarted?.Invoke();
        }
        
        public void EndLevel()
        {
            InputProvider.Disable();
        }
        
        private void Lose()
        {
            EndLevel();
            OnLevelFailed?.Invoke();
        }
        
        private void Win()
        {
            EndLevel();
            OnLevelCompleted?.Invoke();
        }
    }
}