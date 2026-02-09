namespace MineSweeper.UI
{
    using System;
    using System.Collections;
    using Gameplay;
    using UnityEngine;
    using Zenject;

    public class UITimer : MonoBehaviour
    {
        [Inject] 
        private LevelService LevelService { get; set; }
        [Inject]
        private GridService GridService { get; set; }

        [SerializeField]
        private UINumericSprites numericSprites;

        private bool _isActive;
        private Coroutine _coroutine;

        private void OnEnable()
        {
            GridService.OnGridCreated += Reset;
            LevelService.OnLevelStarted += StartTimer;
            LevelService.OnLevelCompleted += StopTimer;
            LevelService.OnLevelFailed += StopTimer;
        }

        private void OnDisable()
        {
            GridService.OnGridCreated -= Reset;
            LevelService.OnLevelStarted -= StartTimer;
            LevelService.OnLevelCompleted -= StopTimer;
            LevelService.OnLevelFailed -= StopTimer;
        }

        private void StartTimer()
        {
            Reset();
            _isActive = true;
            _coroutine = StartCoroutine(TimerRoutine());
        }

        private void Reset()
        {
            if(_coroutine  != null)
                StopCoroutine(_coroutine);
            
            numericSprites.ResetValue();
        }

        private void StopTimer()
        {
            StopCoroutine(_coroutine);
            _isActive = false;
        }

        private IEnumerator TimerRoutine()
        {
            var delay = new WaitForSeconds(1f);
            var seconds = 0;
            
            while (_isActive)
            {
                numericSprites.SetValue(seconds);
                seconds++;
                yield return delay;
            }
        }
    }
}