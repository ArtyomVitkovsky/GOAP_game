using Game.CameraSystem;
using Game.Character.Components;
using Game.NpcSystem;
using Services.GameInputService;
using Services.InteractionService;
using Services.NpcLifeService;
using Services.PlayerControlService;
using Services.ReputationService;
using Services.SavingService;
using Services.TickableService;
using Services.TurretsService;
using Services.VehicleService;
using Zenject;

namespace Services
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BootstrapServiceInstaller.Install(Container);
            SignalBusInstaller.Install(Container);
            
            TickableServiceInstaller.Install(Container);
            
            ReputationServiceInstaller.Install(Container);
            NpcLifeServiceInstaller.Install(Container);
            SavingServiceInstaller.Install(Container);
            ActionPlaningServiceInstaller.Install(Container);
            PlayerControlServiceInstaller.Install(Container);
            CombatServiceInstaller.Install(Container);
            CameraServiceInstaller.Install(Container);
            InteractionServiceInstaller.Install(Container);
            VehicleSettingsServiceInstaller.Install(Container);
            TurretsServiceInstaller.Install(Container);
        }
    }
}
