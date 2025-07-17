using Game.Character.Components;
using Services.PlayerControlService;
using UnityEngine;
using Zenject;

namespace Game.CameraSystem
{
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
}