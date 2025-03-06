using Zenject;

namespace Services.TickableService
{
    public class TickableServiceInstaller : Installer<TickableServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TickableService>().AsSingle().NonLazy();
        }
    }
}