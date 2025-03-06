using Game.Car.Components;
using Zenject;

namespace Game.Car.Installers
{
    public class VehicleCharacterPositionsComponentInstaller : Installer<VehicleCharacterPositionsComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<VehicleCharacterPositionsComponent>().AsSingle().NonLazy();
        }
    }
}