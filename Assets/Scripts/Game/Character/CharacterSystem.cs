using Cinemachine;
using Cysharp.Threading.Tasks;
using Game.CameraSystem;
using Game.Character.Components;
using Game.Character.Installers;
using Game.NpcSystem;
using Services;
using Services.PlayerControlService;
using UnityEngine;
using Zenject;

namespace Game.Character
{
    public class CharacterSystem : MonoBehaviour, IControllable, IWorldMember
    {
        [Inject] private IBootstrapService bootstrapService;
        [Inject] private IPlayerControlService playerControlService;
        [Inject] private ICameraService cameraService;
        
        [Inject] private CharacterControlComponent characterControl;
        [Inject] private CharacterInteractionComponent interactionComponent;

        private Transform parent;
        
        public Transform ControllableTransform => characterControl.Transform;
        public bool IsActive { get; private set; }

        public Transform Transform { get; }
        
        public string Name { get; }
        public Fraction Fraction { get; }
        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }

        public void ResetParent()
        {
            transform.parent = parent;
        }

        public void Start()
        {
            Initialize();
        }

        private async UniTask Initialize()
        {
            await bootstrapService.BootstrapTask;
            
            parent = transform.parent;
            
            cameraService.RequestCameraTypeChange(GameCameraType.Character);
            
            playerControlService.SetCharacterControllable(this);
            playerControlService.SetCurrentControllable(this);
            
            characterControl.Initialize();
            interactionComponent.Initialize(this);
        }

        public void Control()
        {
            if (!IsActive) return;
            
            characterControl.Control();
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            
            characterControl.SetActive(isActive);
            interactionComponent.SetActive(isActive);
        }
    }
}