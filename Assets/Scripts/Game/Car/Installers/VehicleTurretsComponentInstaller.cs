using Game.Car.Components;
using Zenject;

namespace Game.Car.Installers
{
    public class VehicleTurretsComponentInstaller : Installer<VehicleTurretsComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<VehiclePlayerTurretsComponent>().AsSingle().NonLazy();
        }
    }
}