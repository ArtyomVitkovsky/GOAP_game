namespace Game.CameraSystem
{
    public interface ICameraService
    {
        public void Bootstrap();
        public void EnqueueCameraSystem(CameraSystem cameraSystem);
        public void DequeueCameraSystem();
        public CameraSetup RequestCameraTypeChange(GameCameraType gameCameraType);
    }
}