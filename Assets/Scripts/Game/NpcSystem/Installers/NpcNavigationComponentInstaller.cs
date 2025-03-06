using Game.NpcSystem.Components;
using Zenject;

namespace Game.NpcSystem.Installers
{
    public class NpcNavigationComponentInstaller : Installer<NpcNavigationComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NpcNavigationComponent>().AsSingle().NonLazy();
        }
    }
}