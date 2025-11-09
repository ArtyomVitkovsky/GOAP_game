using Configs.PlayerConfig;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Game.CameraSystem.Components
{
    public class CameraZoomComponent
    {
        [Inject] private PlayerMovementBaseStats playerMovementBaseStats;

        private Vector3 followOffset;
        private Vector3 zoomDirection;

        private float middleRadius;

        private float zoomFactor;
        private CinemachineThirdPersonFollow transposer;

        public float ZoomFactor => zoomFactor;
        
        public void HandleCameraZoom(CameraSetup currentCamera)
        {
            // switch (currentCamera.Cinemachine.)
            // {
            //     case CinemachineCamera virtualCamera:
            //     {
            //         VirtualCameraZoom(virtualCamera);
            //         break;
            //     }
            //     case CinemachineCamera freeLook:
            //     {
            //         FreeLookCameraZoom(freeLook);
            //         break;
            //     }
            // }
        }

        private void VirtualCameraZoom(CinemachineCamera virtualCamera)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                followOffset.z += Input.mouseScrollDelta.y;
            }

            followOffset.z = Mathf.Clamp(
                followOffset.z,
                Mathf.Min(playerMovementBaseStats.MinOffset, playerMovementBaseStats.MaxOffset),
                Mathf.Max(playerMovementBaseStats.MinOffset, playerMovementBaseStats.MaxOffset)
            );

            if (transposer == null)
            {
                if (transposer == null) return;
            }

            followOffset.x = transposer.ShoulderOffset.x;
            followOffset.y = transposer.ShoulderOffset.y;

            transposer.ShoulderOffset = Vector3.Lerp(
                transposer.ShoulderOffset, 
                followOffset,
                Time.deltaTime * playerMovementBaseStats.ZoomSpeed);

            zoomFactor = Mathf.Abs(transposer.ShoulderOffset.z - playerMovementBaseStats.MinOffset) /
                         Mathf.Abs(playerMovementBaseStats.MaxOffset - playerMovementBaseStats.MinOffset);
            zoomFactor = Mathf.Clamp(zoomFactor, 0, 1);
        }

        private void FreeLookCameraZoom(CinemachineFreeLook freeLook)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                middleRadius -= Input.mouseScrollDelta.y;
            }

            if (middleRadius > playerMovementBaseStats.MaxOffset)
            {
                middleRadius = playerMovementBaseStats.MaxOffset;
            }

            if (middleRadius < playerMovementBaseStats.MinOffset)
            {
                middleRadius = playerMovementBaseStats.MinOffset;
            }

            zoomFactor = middleRadius / (playerMovementBaseStats.MaxOffset - playerMovementBaseStats.MinOffset);

            freeLook.m_Orbits[1].m_Radius = middleRadius;
        }
    }
}