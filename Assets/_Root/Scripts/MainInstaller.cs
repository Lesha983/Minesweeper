namespace MineSweeper
{
    using Gameplay;
    using UnityEngine;
    using Zenject;

    public class MainInstaller : MonoInstaller
    {
        [SerializeField]
        private GridService gridService;
        
        private static readonly string SettingsPath = "Settings";
        private static readonly string GridSettingsPath = SettingsPath + "/GridSettings";
        
        public override void InstallBindings()
        {
            BindSettings();
            Bind();
        }

        private void BindSettings()
        {
            Container.Bind<GridSettings>().FromResource(GridSettingsPath).AsSingle();
        }

        private void Bind()
        {
            Container.Bind<GridService>().FromInstance(gridService).AsSingle();
        }
    }
}