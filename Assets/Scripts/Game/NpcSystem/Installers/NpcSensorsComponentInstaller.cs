using Game.NpcSystem.Components;
using Zenject;

namespace Game.NpcSystem.Installers
{
    public class NpcSensorsComponentInstaller : Installer<NpcSensorsComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NpcSensorsComponent>().AsSingle().NonLazy();
        }
    }
}