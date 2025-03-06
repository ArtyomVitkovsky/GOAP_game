namespace Services.GameInputService
{
    public interface IGameInputService
    {
        public void Bootstrap();
        
        public void AddPlayerCallbacks(InputSystem_Actions.IPlayerActions playerActions);
    
        public void AddVehicleCallbacks(InputSystem_Actions.IVehicleActions vehicleActions);
    }
}