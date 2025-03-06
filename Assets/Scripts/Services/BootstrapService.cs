using Cysharp.Threading.Tasks;
using Game.CameraSystem;
using Game.Character.Components;
using Game.NpcSystem;
using Services.GameInputService;
using Services.InteractionService;
using Services.NpcLifeService;
using Services.PlayerControlService;
using Services.SavingService;
using Services.TickableService;
using Services.TurretsService;
using Services.VehicleService;
using Zenject;

namespace Services
{
    public interface IBootstrapService
    {
        public UniTask BootstrapTask { get; }
    }

    public class BootstrapServiceInstaller : Installer<BootstrapServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BootstrapService>().AsSingle().NonLazy();
        }
    }
    
    public class BootstrapService : IBootstrapService, IInitializable
    {
        private UniTaskCompletionSource BootstrapUCS = new UniTaskCompletionSource();
        
        public UniTask BootstrapTask => BootstrapUCS.Task;

        private ITickableService tickableService;
        private IGameInputService gameInputService;
        private IPlayerControlService playerControlService;
        private IActionPlaningService actionPlaningService;
        private INpcLifeService npcLifeService;
        private ICameraService cameraService;
        private IInteractionService interactionService;
        private IPlayerTurretsService playerTurretsService;
        private IVehicleSettingsService vehicleSettingsService;
        private ISavingService savingService;

        [Inject]
        public BootstrapService(
            ITickableService tickableService,
            IGameInputService gameInputService,
            IPlayerControlService playerControlService,
            IActionPlaningService actionPlaningService,
            INpcLifeService npcLifeService,
            ICameraService cameraService,
            IInteractionService interactionService,
            IPlayerTurretsService playerTurretsService,
            IVehicleSettingsService vehicleSettingsService,
            ISavingService savingService
        )
        {
            this.tickableService = tickableService;
            this.gameInputService = gameInputService;
            this.playerControlService = playerControlService;
            this.actionPlaningService = actionPlaningService;
            this.npcLifeService = npcLifeService;
            this.cameraService = cameraService;
            this.interactionService = interactionService;
            this.playerTurretsService = playerTurretsService;
            this.vehicleSettingsService = vehicleSettingsService;
            this.savingService = savingService;
        }
        
        public void Initialize()
        {
            savingService.Bootstrap();
            
            tickableService.Bootstrap();
            gameInputService.Bootstrap();
            playerControlService.Bootstrap();
            actionPlaningService.Bootstrap();
            npcLifeService.Bootstrap();
            cameraService.Bootstrap();
            interactionService.Bootstrap();
            playerTurretsService.Bootstrap();
            vehicleSettingsService.Bootstrap();
            savingService.Bootstrap();

            BootstrapUCS?.TrySetResult();
        }

    }
}