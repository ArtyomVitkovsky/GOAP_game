using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Game.CameraSystem;
using Game.Car.Components;
using Game.Car.Installers;
using Game.Character;
using Game.NpcSystem;
using Services;
using Services.InteractionService;
using Services.PlayerControlService;
using UnityEngine;
using Zenject;

namespace Game.Car
{
    [Serializable]
    public enum Axel
    {
        Front = 0,
        Rear = 1,
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject view;
        public WheelCollider collider;
        public Axel axel;
    }

    public enum VehicleCharacterPositionType
    {
        EnterExit = 0,
        DriverSeat = 1,
        PassengerSeat = 2
    }

    public enum VehicleOwnerType
    {
        Player = 0,
        NPC = 1
    }
    
    public class VehicleSystem : MonoBehaviour, IControllable, IInteractable
    {
        [Inject] private DiContainer diContainer;
        
        [Inject] private IBootstrapService bootstrapService;
        [Inject] private IPlayerControlService playerControlService;
        [Inject] private ICameraService cameraService;
        [Inject] private PlayerVehicleControlComponent playerControlComponent;
        [Inject] private NpcVehicleControlComponent npcControlComponent;
        [Inject] private VehicleTransmissionComponent transmissionComponent;
        [Inject] private VehicleCharacterPositionsComponent сharacterPositionsComponent;
        
        [Inject(Id = VehicleSystemInstaller.VEHICLE_TRANSFORM)]
        private Transform transform;
        
        [Inject(Id = VehicleSystemInstaller.VEHICLE_FOLLOW_TARGET)]
        private Transform followTarget;
        [Inject(Id = VehicleSystemInstaller.VEHICLE_LOOK_TARGET)]
        private Transform lookTarget;
        
        private VehicleTurretsComponent _turretsComponent;
        
        private VehicleActionsProvider vehicleActionsProvider;
        
        private bool exitTimeOutEnded;
        
        private bool isInitialized;

        private VehicleControlComponent vehicleControlComponent;
        private IWorldMember owner;
        private VehicleOwnerType ownerType;
        
        private float notGroundedTime = 0;

        public string ownerName;

        public Transform ControllableTransform => transform;
        public Transform InteractableTransform => transform;

        public IWorldMember Owner => owner;

        
        public bool IsActive { get; private set; }

        public bool IsNpcControlPossible()
        {
            var isGrounded = npcControlComponent.IsAllWheelsGrounded() || 
                             npcControlComponent.IsHalfOfWheelsGrounded();

            if (isGrounded)
            {
                notGroundedTime = 0;
            }
            else
            {
                notGroundedTime += Time.deltaTime;
            }

            return notGroundedTime < 10;
        }

        private void Awake()
        {
            npcControlComponent.Stop();
            playerControlComponent.Stop();
        }

        private async UniTask InitializeForPlayer()
        {
            await bootstrapService.BootstrapTask;

            transmissionComponent.Initialize();
            
            vehicleActionsProvider = playerControlService.VehicleActionsProvider;
            vehicleActionsProvider.OnExitAction += OnExitAction;
        }

        public void Control()
        {
            if (!IsActive) return;
            
            vehicleControlComponent.Control();
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void ResetPosition()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            transform.eulerAngles = Vector3.zero;
        }
        
        public void Interact(IWorldMember interactionInitiator)
        {
            if (interactionInitiator is CharacterSystem characterSystem)
            {
                interactionInitiator = characterSystem;
                ownerType = VehicleOwnerType.Player;
                vehicleControlComponent = playerControlComponent;
                vehicleControlComponent.Initialize();

                OnPlayerInteract(interactionInitiator);
            }
            else if (interactionInitiator is NpcCharacter npc)
            {
                ownerType = VehicleOwnerType.NPC;
                npcControlComponent.Setup(npc.NavigationComponent);
                npc.NavigationComponent.VehicleControl = npcControlComponent;
                vehicleControlComponent = npcControlComponent;
                vehicleControlComponent.Initialize();
            }
            
            owner = interactionInitiator;
            ownerName = owner.Name;
            сharacterPositionsComponent.SetCharacterTo(interactionInitiator, VehicleCharacterPositionType.DriverSeat);
        }

        private async UniTask OnPlayerInteract(IWorldMember interactionInitiator)
        {
            await InitializeForPlayer();
            
            playerControlService.SetCurrentControllable(this);
            var camera = cameraService.RequestCameraTypeChange(GameCameraType.Vehicle);
            camera.Cinemachine.Follow = followTarget;
            camera.Cinemachine.LookAt = lookTarget;

            _turretsComponent = diContainer.Resolve<VehiclePlayerTurretsComponent>();
            _turretsComponent.Initialize(interactionInitiator);
            
            ExitTimeout();
        }
        
        private void OnExitAction()
        {
            if (!exitTimeOutEnded) return;
            
            _turretsComponent.Dispose();
            
            OwnerExit();
            
            playerControlService.ResetCurrentControllable();
            cameraService.RequestCameraTypeChange(GameCameraType.Character);

            exitTimeOutEnded = false;
        }

        private void OwnerExit()
        {
            сharacterPositionsComponent.SetCharacterTo(owner, VehicleCharacterPositionType.EnterExit);
            vehicleControlComponent.Dispose();
            vehicleControlComponent = null;
            owner = null;
            ownerName = null;
        }

        public void Exit()
        {
            OwnerExit();
        }


        public bool Stop()
        {
            return vehicleControlComponent.Stop();
        }

        private async UniTask ExitTimeout()
        {
            await UniTask.Delay(1000);

            exitTimeOutEnded = true;
        }
    }
}