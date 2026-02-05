namespace MineSweeper.Gameplay
{
    using UnityEngine;
    using Zenject;

    public class LevelStarter : MonoBehaviour
    {
        [Inject]
        private LevelService LevelService { get; set; }
        
        private void Start()
        {
            LevelService.StartLevel();
        }
    }
}