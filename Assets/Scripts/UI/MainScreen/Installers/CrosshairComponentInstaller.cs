using UI.MainScreen.Components;
using Zenject;

namespace UI.MainScreen.Installers
{
    public class CrosshairComponentInstaller : Installer<CrosshairComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CrosshairComponent>().AsSingle().NonLazy();
        }
    }
}