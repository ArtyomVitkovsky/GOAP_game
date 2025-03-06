using System.Collections.Generic;
using Zenject;

namespace Game.CameraSystem
{
    public class CameraService : ICameraService
    {
        [Inject] private SignalBus _signalBus;

        private Queue<CameraSystem> CameraSystems;

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

        public void RequestCameraTypeChange(GameCameraType gameCameraType)
        {
            if (CameraSystems.TryPeek(out var cameraSystem))
            {
                cameraSystem.ChangeCamera(gameCameraType);
            }
        }
    }
}