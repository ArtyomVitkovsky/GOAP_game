using System;
using Game.CameraSystem.Installers;
using Services.InteractionService;
using UnityEngine;
using Zenject;

namespace Game.CameraSystem.Components
{
    public class CameraRayCastComponent
    {
        [Inject] private SignalBus signalBus;
        
        [Inject(Id = CameraSystemInstaller.GAMEPLAY_CAMERA)] private Camera camera;
        [Inject(Id = CameraSystemInstaller.CAMERA_RAYCAST_POINTER)] private Transform cameraRaycastPointer;
        [Inject(Id = CameraSystemInstaller.RAYCAST_LAYER_MASK)] private LayerMask layerMask;

        private Vector3 rayOrigin;
        private RaycastHit rayHit;

        private bool isInitialized;

        public void Initialize()
        {
            rayOrigin = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            isInitialized = true;
        }

        public void Raycast()
        {
            if (!isInitialized) return;
            
            var ray = camera.ScreenPointToRay(rayOrigin);
            
            Debug.DrawRay(ray.origin, ray.direction * 5, Color.blue);

            if (Physics.Raycast(ray, out rayHit, float.PositiveInfinity, ~layerMask))
            {
                cameraRaycastPointer.position = rayHit.point;
            }
        }
    }
}