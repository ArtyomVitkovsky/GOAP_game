using Game.Car.Components;
using Zenject;

namespace Game.Car.Installers
{
    public class VehicleTransmissionComponentInstaller : Installer<VehicleTransmissionComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<VehicleTransmissionComponent>().AsSingle().NonLazy();
        }
    }
}