using Game.NpcSystem;
using UnityEngine;

namespace Services.InteractionService
{
    public interface IInteractable
    {
        public Transform InteractableTransform { get; }
        
        public void Interact(IWorldMember interactionInitiator);
    }
}