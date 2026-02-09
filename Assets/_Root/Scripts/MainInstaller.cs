namespace MineSweeper
{
    using Gameplay;
    using Input;
    using UnityEngine;
    using Zenject;

    public class MainInstaller : MonoInstaller
    {
        [SerializeField]
        private CellsGrid cellsGrid;
        [SerializeField]
        private InputToCellService inputToCellService;
        
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
            Container.BindInterfacesAndSelfTo<InputProvider>().AsSingle();
            
            Container.Bind<GridService>().AsSingle();
            Container.Bind<CellsGrid>().FromInstance(cellsGrid).AsSingle();
            Container.Bind<IInputToCell>().FromInstance(inputToCellService).AsSingle();
            Container.BindInterfacesAndSelfTo<OpenCellService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelService>().AsSingle();
        }
    }
}