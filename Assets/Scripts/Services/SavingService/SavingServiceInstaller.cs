using Zenject;

namespace Services.SavingService
{
    public class SavingServiceInstaller : Installer<SavingServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SavingService>().AsSingle().NonLazy();
        }
    }
}