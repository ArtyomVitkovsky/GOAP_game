using Zenject;

namespace UI.MainScreen
{
    public class InteractionUserInterfaceComponentInstaller : Installer<InteractionUserInterfaceComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InteractionUserInterfaceComponent>().AsSingle().NonLazy();
        }
    }
}