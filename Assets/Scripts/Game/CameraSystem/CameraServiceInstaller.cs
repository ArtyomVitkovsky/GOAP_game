using Zenject;

namespace Game.CameraSystem
{
    public class CameraServiceInstaller : Installer<CameraServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraService>().AsSingle().NonLazy();
        }
    }
}