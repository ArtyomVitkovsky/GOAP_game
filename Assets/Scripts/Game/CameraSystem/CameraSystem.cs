using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.CameraSystem.Components;
using Services;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Game.CameraSystem
{
    public enum GameCameraType
    {
        Character = 0,
        Vehicle = 1,
    }

    public class CameraSystem : MonoBehaviour
    {
        [Inject] private IBootstrapService bootstrapService;
        [Inject] private ICameraService cameraService;
        [Inject] private ITickableService tickableService;

        [Inject] private CameraZoomComponent zoomComponent;
        [Inject] private CameraRayCastComponent rayCastComponent;

        [Inject] private List<CameraSetup> cameraSetups;

        private CameraSetup currentCamera;

        public CameraSetup CurrentCamera => currentCamera;

        private void Awake()
        {
            Initialize();
        }

        private async UniTask Initialize()
        {
            await bootstrapService.BootstrapTask;
            
            cameraService.EnqueueCameraSystem(this);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            tickableService.AddUpdateTickable(new TickableEntity(Control));

            rayCastComponent.Initialize();
        }

        public void ChangeCamera(GameCameraType gameCameraType)
        {
            var targetSetup = cameraSetups.FirstOrDefault(c => c.GameCameraType == gameCameraType);

            if (targetSetup == null) return;

            currentCamera = targetSetup;

            foreach (var cameraSetup in cameraSetups)
            {
                cameraSetup.Cinemachine.enabled = cameraSetup.GameCameraType == gameCameraType;
            }
        }

        private void Control()
        {
            zoomComponent.HandleCameraZoom(currentCamera);
            rayCastComponent.Raycast();
        }

        private void OnDestroy()
        {
            cameraService.DequeueCameraSystem();
        }
    }
}