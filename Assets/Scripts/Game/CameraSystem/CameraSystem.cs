using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.CameraSystem.Components;
using Game.Character.Components;
using Services;
using Services.PlayerControlService;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Game.CameraSystem
{
    public enum GameCameraType
    {
        Character = 0,
        Vehicle = 1,
        CharacterCombat = 2
    }
    
    public class CombatCameraComponent
    {
        [Inject] private IPlayerControlService playerControlService;

        private CameraSetup combatCamera;
        private Transform combatCameraTransform => combatCamera.Cinemachine.transform;

        private Quaternion targeRotation;
        
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private float TopClamp = 70;
        private float BottomClamp = -30;

        private PlayerActionsProvider PlayerActionsProvider => playerControlService.PlayerActionsProvider;

        public void Initialize(CameraSetup combatCamera)
        {
            this.combatCamera = combatCamera;
        }

        public void ProcessAimingRotation(CameraSetup currentCamera)
        {
            if (combatCamera == null || PlayerActionsProvider == null || !PlayerActionsProvider.Aim) return;
            if (currentCamera.GameCameraType != combatCamera.GameCameraType) return;
            
            if (PlayerActionsProvider.LookVector.sqrMagnitude >= 0.01f)
            {
                float deltaTimeMultiplier = 0.25f;
            
                _cinemachineTargetYaw += PlayerActionsProvider.LookVector.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += -PlayerActionsProvider.LookVector.y * deltaTimeMultiplier;
            }
            
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            
            combatCamera.Cinemachine.Follow.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }

    public class CameraSystem : MonoBehaviour
    {
        [Inject] private IBootstrapService bootstrapService;
        [Inject] private ICameraService cameraService;
        [Inject] private ITickableService tickableService;

        [Inject] private CameraZoomComponent zoomComponent;
        [Inject] private CameraRayCastComponent rayCastComponent;
        [Inject] private CombatCameraComponent combatCameraComponent;

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

            var combatCamera = cameraSetups.FirstOrDefault(c => c.GameCameraType == GameCameraType.CharacterCombat);
            combatCameraComponent.Initialize(combatCamera);
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
            combatCameraComponent.ProcessAimingRotation(currentCamera);
        }

        private void OnDestroy()
        {
            cameraService.DequeueCameraSystem();
        }
    }
}