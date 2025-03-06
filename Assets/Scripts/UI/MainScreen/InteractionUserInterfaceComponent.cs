using Game.CameraSystem.Installers;
using Services.InteractionService;
using Services.TickableService;
using UI.MainScreen.Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainScreen
{
    public class InteractionUserInterfaceComponent
    {
        [Inject] private ITickableService tickableService;
        [Inject] private IInteractionService interactionService;

        [Inject(Id = MainScreenInstaller.INTERACT_IMAGE)] 
        private Image interactImage;
        
        [Inject(Id = CameraSystemInstaller.GAMEPLAY_CAMERA)]
        private Camera camera;
        
        private IInteractable currentInteractable;
        
        public void Initialize()
        {
            interactImage.gameObject.SetActive(false);

            tickableService.AddUpdateTickable(new TickableEntity(UpdateInteractionButtonPosition));

            interactionService.OnCurrentInteractableChanged += OnCurrentInteractableChanged;
        }
        
        private void OnCurrentInteractableChanged(IInteractable interactable)
        {
            currentInteractable = interactable;
            interactImage.gameObject.SetActive(interactable != null);
        }

        private void UpdateInteractionButtonPosition()
        {
            if (currentInteractable == null) return;

            interactImage.rectTransform.position = 
                camera.WorldToScreenPoint(currentInteractable.InteractableTransform.position);

        }
    }
}