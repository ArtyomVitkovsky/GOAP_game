using Cinemachine;
using Game.CameraSystem.Installers;
using UnityEngine;
using Zenject;

namespace Game.CameraSystem.Components
{
    public class CameraZoomComponent
    {
        [Inject(Id = CameraSystemInstaller.MAX_OFFSET)] private float maxOffest;
        [Inject(Id = CameraSystemInstaller.MIN_OFFSET)] private float minOffest;
        [Inject(Id = CameraSystemInstaller.ZOOM_SPEED)] private float zoomSpeed;

        private Vector3 followOffset;
        private Vector3 zoomDirection;

        private float middleRadius;
        
        public void HandleCameraZoom(CameraSetup currentCamera)
        {
            if (currentCamera.GameCameraType == GameCameraType.CharacterCombat) return;
            
            switch (currentCamera.Cinemachine)
            {
                case CinemachineVirtualCamera virtualCamera:
                {
                    VirtualCameraZoom(virtualCamera);
                    break;
                }
                case CinemachineFreeLook freeLook:
                {
                    FreeLookCameraZoom(freeLook);
                    break;
                }
            }
        }

        private void VirtualCameraZoom(CinemachineVirtualCamera virtualCamera)
        {
            zoomDirection = followOffset.normalized;
            
            if (Input.mouseScrollDelta.y != 0)
            {
                followOffset += zoomDirection * -Input.mouseScrollDelta.y;
            }

            if (followOffset.magnitude > maxOffest)
            {
                followOffset = zoomDirection * maxOffest;
            }

            if (followOffset.magnitude < minOffest)
            {
                followOffset = zoomDirection * minOffest;
            }

            var transposer = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if (transposer == null) return;
            
            transposer.ShoulderOffset = Vector3.Lerp(transposer.ShoulderOffset, followOffset, Time.deltaTime * zoomSpeed);
        }

        private void FreeLookCameraZoom(CinemachineFreeLook freeLook)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                middleRadius -= Input.mouseScrollDelta.y;
            }

            if (middleRadius > maxOffest)
            {
                middleRadius = maxOffest;
            }

            if (middleRadius < minOffest)
            {
                middleRadius = minOffest;
            }

            freeLook.m_Orbits[1].m_Radius = middleRadius;
        }
    }
}