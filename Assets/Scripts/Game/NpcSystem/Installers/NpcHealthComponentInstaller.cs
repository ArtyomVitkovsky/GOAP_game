using Game.NpcSystem.Components;
using Zenject;

namespace Game.NpcSystem.Installers
{
    public class NpcHealthComponentInstaller : Installer<NpcHealthComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NpcHealthComponent>().AsSingle().NonLazy();
        }
    }
}