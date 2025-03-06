using UnityEngine.InputSystem;
using Zenject;

namespace Services.GameInputService
{
    public class GameInputService : IGameInputService
    {
        [Inject] private PlayerInput playerInput;

        private InputSystem_Actions inputSystemActions;
        
        public void Bootstrap()
        {
            inputSystemActions = new InputSystem_Actions();
            inputSystemActions.Enable();
        }

        public void AddPlayerCallbacks(InputSystem_Actions.IPlayerActions playerActions)
        {
            inputSystemActions.Player.AddCallbacks(playerActions);
        }

        public void AddVehicleCallbacks(InputSystem_Actions.IVehicleActions vehicleActions)
        {
            inputSystemActions.Vehicle.AddCallbacks(vehicleActions);
        }
    }
}