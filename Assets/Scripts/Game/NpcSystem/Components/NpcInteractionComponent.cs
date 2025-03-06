using Game.Car;
using Game.Character.Components;
using Game.Character.Installers;
using Game.NpcSystem.Installers;
using Services.InteractionService;
using UnityEngine;
using Zenject;

namespace Game.NpcSystem.Components
{
    public class NpcInteractionComponent : InteractionComponent
    {
        [Inject(Id = NpcCharacterInstaller.CHARACTER_INTERACTION_SETUP)]
        private CharacterInteractionSetup interactionSetup;
        
        [Inject(Id = NpcCharacterInstaller.CHARACTER_INTERACTION_ORIGIN_POINT)]
        private Transform originPoint;

        protected override CharacterInteractionSetup InteractionSetup => interactionSetup;
        protected override Transform OriginPoint => originPoint;
        
        public bool TryToInteract<T>(out T target) where T : IInteractable
        {
            target = default;
            
            if (interactable != null && interactable is VehicleSystem vehicle)
            {
                if (vehicle.Owner != null) return false;

                target = interactable is T result ? result : default;
                interactable.Interact(interactor);
                return true;
            }

            return false;
        }
    }
}