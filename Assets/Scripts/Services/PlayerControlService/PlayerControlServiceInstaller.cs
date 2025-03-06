using Zenject;

namespace Services.PlayerControlService
{
    public class PlayerControlServiceInstaller : Installer<PlayerControlServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerControlService>().AsSingle().NonLazy();
        }
    }
}