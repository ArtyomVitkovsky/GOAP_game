using Zenject;

namespace Services.VehicleService
{
    public class VehicleSettingsServiceInstaller : Installer<VehicleSettingsServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<VehicleSettingsService>().AsSingle().NonLazy();
        }
    }
}