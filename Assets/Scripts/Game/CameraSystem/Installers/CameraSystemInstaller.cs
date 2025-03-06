using System.Collections.Generic;
using Cinemachine;
using Game.CameraSystem.Components;
using Services.InteractionService;
using UnityEngine;
using Zenject;

namespace Game.CameraSystem.Installers
{
    public class CameraSystemInstaller : MonoInstaller
    {
        public const string MAX_OFFSET = "MAX_OFFSET";
        public const string MIN_OFFSET = "MIN_OFFSET";
        public const string ZOOM_SPEED = "ZOOM_SPEED";
        public const string GAMEPLAY_CAMERA = "GAMEPLAY_CAMERA";
        public const string CAMERA_RAYCAST_POINTER = "CAMERA_RAYCAST_POINTER";
        public const string RAYCAST_LAYER_MASK = "RAYCAST_LAYER_MASK";

        [Inject] private SignalBus signalBus;
        
        [Header("CameraSetup")]
        [SerializeField] private Camera camera;
        [SerializeField] private List<CameraSetup> cameraSetups;
        
        [Header("Raycast")]
        [SerializeField] private Transform cameraRaycastPointer;
        [SerializeField] private LayerMask layerMask;
        
        [Header("Zoom")]
        [SerializeField] private float maxOffset;
        [SerializeField] private float minOffset;
        [SerializeField] private float zoomSpeed;

        public override void InstallBindings()
        {
            BindInstances();

            BindComponents();
        }

        private void BindComponents()
        {
            Container.BindInterfacesAndSelfTo<CameraZoomComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CameraRayCastComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CombatCameraComponent>().AsSingle().NonLazy();
        }

        private void BindInstances()
        {
            Container.BindInstance(camera).WithId(GAMEPLAY_CAMERA);
            Container.BindInstance(cameraSetups);
            
            Container.BindInstance(cameraRaycastPointer).WithId(CAMERA_RAYCAST_POINTER);
            Container.BindInstance(layerMask).WithId(RAYCAST_LAYER_MASK);

            Container.BindInstance(maxOffset).WithId(MAX_OFFSET);
            Container.BindInstance(minOffset).WithId(MIN_OFFSET);
            Container.BindInstance(zoomSpeed).WithId(ZOOM_SPEED);
        }
    }
}