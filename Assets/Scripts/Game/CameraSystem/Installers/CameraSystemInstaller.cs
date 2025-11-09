using System.Collections.Generic;
using Game.CameraSystem.Components;
using UnityEngine;
using Zenject;

namespace Game.CameraSystem.Installers
{
    public class CameraSystemInstaller : MonoInstaller
    {
        public const string GAMEPLAY_CAMERA = "GAMEPLAY_CAMERA";
        public const string CAMERA_RAYCAST_POINTER = "CAMERA_RAYCAST_POINTER";
        public const string RAYCAST_LAYER_MASK = "RAYCAST_LAYER_MASK";

        [Inject] private SignalBus signalBus;
        
        [SerializeField] private CameraSystem cameraSystem;
        
        [Header("CameraSetup")]
        [SerializeField] private Camera camera;
        [SerializeField] private List<CameraSetup> cameraSetups;
        
        [Header("Raycast")]
        [SerializeField] private Transform cameraRaycastPointer;
        [SerializeField] private LayerMask layerMask;

        public override void InstallBindings()
        {
            BindInstances();

            BindComponents();

            Container.BindInstance(cameraSystem);
        }

        private void BindComponents()
        {
            Container.BindInterfacesAndSelfTo<CameraZoomComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CameraRayCastComponent>().AsSingle().NonLazy();
        }

        private void BindInstances()
        {
            Container.BindInstance(camera).WithId(GAMEPLAY_CAMERA);
            Container.BindInstance(cameraSetups);
            
            Container.BindInstance(cameraRaycastPointer).WithId(CAMERA_RAYCAST_POINTER);
            Container.BindInstance(layerMask).WithId(RAYCAST_LAYER_MASK);
        }
    }
}