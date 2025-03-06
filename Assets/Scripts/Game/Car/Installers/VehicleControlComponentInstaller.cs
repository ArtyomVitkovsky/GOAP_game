using Game.Car.Components;
using Zenject;

namespace Game.Car.Installers
{
    public class PlayerVehicleControlComponentInstaller : Installer<PlayerVehicleControlComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerVehicleControlComponent>().AsSingle().NonLazy();
        }
    }
    
    public class NpcVehicleControlComponentInstaller : Installer<NpcVehicleControlComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NpcVehicleControlComponent>().AsSingle().NonLazy();
        }
    }
}