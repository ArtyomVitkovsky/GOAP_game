using Game.NpcSystem.Components;
using Zenject;

namespace Game.NpcSystem.Installers
{
    public class NpcMoodComponentInstaller : Installer<NpcMoodComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NpcMoodComponent>().AsSingle().NonLazy();
        }
    }
}