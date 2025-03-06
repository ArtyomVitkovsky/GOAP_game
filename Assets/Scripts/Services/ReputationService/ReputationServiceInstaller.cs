using Zenject;

namespace Services.ReputationService
{
    public class ReputationServiceInstaller : Installer<ReputationServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ReputationService>().AsSingle().NonLazy();
        }
    }
}