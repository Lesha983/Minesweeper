namespace MineSweeper.Gameplay
{
    using System;
    using Input;
    using Zenject;

    public class LevelService : IInitializable, IDisposable
    {
        [Inject]
        private IInputProvider InputProvider { get; set; }
        [Inject]
        private OpenCellService OpenCellService { get; set; }
        [Inject]
        private GridService GridService { get; set; }
        
        public event Action OnLevelCompleted;
        public event Action OnLevelFailed;
        public event Action OnLevelStarted;
        
        public void Initialize()
        {
            GridService.OnGridCreated += GridCreated;
            
            OpenCellService.OnFirstCellClicked += StartLevel;
            OpenCellService.OnMineCellClicked += Lose;
            OpenCellService.OnLevelCompleted += Win;
        }

        public void Dispose()
        {
            GridService.OnGridCreated -= GridCreated;
            
            OpenCellService.OnFirstCellClicked -= StartLevel;
            OpenCellService.OnMineCellClicked -= Lose;
            OpenCellService.OnLevelCompleted -= Win;
        }

        public void StartLevel()
        {
            OnLevelStarted?.Invoke();
        }

        private void GridCreated()
        {
            InputProvider.Enable();
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

        private void EndLevel()
        {
            InputProvider.Disable();
        }
    }
}