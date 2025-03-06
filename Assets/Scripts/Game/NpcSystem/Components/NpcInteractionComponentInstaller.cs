using Zenject;

namespace Game.NpcSystem.Components
{
    public class NpcInteractionComponentInstaller : Installer<NpcInteractionComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NpcInteractionComponent>().AsSingle().NonLazy();
        }
    }
}