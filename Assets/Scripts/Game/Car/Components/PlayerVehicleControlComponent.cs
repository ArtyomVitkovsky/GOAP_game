using Services.PlayerControlService;
using Zenject;

namespace Game.Car.Components
{
    public class PlayerVehicleControlComponent : VehicleControlComponent
    {
        [Inject] private IPlayerControlService playerControlService;
        
        private VehicleActionsProvider vehicleActionsProvider;

        protected override bool IsInitialized { get; set; }

        public override void Initialize()
        {
            vehicleActionsProvider = playerControlService.VehicleActionsProvider;
            ReleaseBreaks();

            IsInitialized = true;
        }

        public override void Dispose()
        {
            vehicleActionsProvider = null;
            IsInitialized = false;
        }

        public override void Control()
        {
            MoveVector = vehicleActionsProvider.MoveVector;
            HandBreak = vehicleActionsProvider.HandBreak;
            
            base.Control();
        }
    }
}