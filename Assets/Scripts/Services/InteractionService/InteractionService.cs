using System;
using Game.Character;
using Game.NpcSystem;
using Services.PlayerControlService;
using UnityEngine;
using Zenject;

namespace Services.InteractionService
{
    public class InteractionService : IInteractionService
    {
        [Inject] private IPlayerControlService playerControlService;
    
        [Inject] private SignalBus signalBus;
    
        public IInteractable CurrentInteractable { get; set; }
        public Action<IInteractable> OnCurrentInteractableChanged { get; set; }

        public void Bootstrap()
        {
            playerControlService.PlayerActionsProvider.OnInteractStartedAction += OnInteractStartedAction;
        }

        public void TrySetInteractable(IInteractable interactable)
        {
            if (CurrentInteractable == interactable) return;
        
            CurrentInteractable = interactable;
            OnCurrentInteractableChanged?.Invoke(CurrentInteractable);
        }

        private void OnInteractStartedAction()
        {
            CurrentInteractable?.Interact(playerControlService.CharacterControllable as IWorldMember);
        }
    }
}