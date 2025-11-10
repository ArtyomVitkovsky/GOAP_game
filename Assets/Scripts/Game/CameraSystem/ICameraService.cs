using Unity.Cinemachine;

namespace Game.CameraSystem
{
    public interface ICameraService
    {
        public CinemachineVirtualCameraBase CurrentCinemachineCamera { get; }
        public void Bootstrap();
        public void EnqueueCameraSystem(CameraSystem cameraSystem);
        public void DequeueCameraSystem();
        public CameraSetup RequestCameraTypeChange(GameCameraType gameCameraType);
    }
}