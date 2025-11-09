using System.Collections.Generic;
using Unity.Cinemachine;
using Zenject;

namespace Game.CameraSystem
{
    public class CameraService : ICameraService
    {
        [Inject] private SignalBus _signalBus;

        private Queue<CameraSystem> CameraSystems;

        public CinemachineVirtualCameraBase CurrentCinemachineCamera { get; private set; }

        public void Bootstrap()
        {
            CameraSystems = new Queue<CameraSystem>();
        }

        public void EnqueueCameraSystem(CameraSystem cameraSystem)
        {
            CameraSystems.Enqueue(cameraSystem);
        }

        public void DequeueCameraSystem()
        {
            CameraSystems.Dequeue();
        }

        public CameraSetup RequestCameraTypeChange(GameCameraType gameCameraType)
        {
            if (CameraSystems.TryPeek(out var cameraSystem))
            {
                cameraSystem.ChangeCamera(gameCameraType);
                CurrentCinemachineCamera = cameraSystem.CurrentCamera.Cinemachine;
                return cameraSystem.CurrentCamera;
            }

            return null;
        }
    }
}