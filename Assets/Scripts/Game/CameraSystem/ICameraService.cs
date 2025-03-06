namespace Game.CameraSystem
{
    public interface ICameraService
    {
        public void Bootstrap();
        public void EnqueueCameraSystem(CameraSystem cameraSystem);
        public void DequeueCameraSystem();
        public void RequestCameraTypeChange(GameCameraType gameCameraType);
    }
}