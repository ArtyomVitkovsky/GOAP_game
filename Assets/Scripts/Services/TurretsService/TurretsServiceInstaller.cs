using Zenject;

namespace Services.TurretsService
{
    public class TurretsServiceInstaller : Installer<TurretsServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerTurretsService>().AsSingle().NonLazy();
        }
    }
}