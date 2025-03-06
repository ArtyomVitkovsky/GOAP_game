using Game.Character.Installers;
using Game.NpcSystem;
using Services.InteractionService;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Game.Character.Components
{
    public abstract class InteractionComponent
    {
        [Inject] private ITickableService tickableService;

        protected abstract CharacterInteractionSetup InteractionSetup { get; }

        protected abstract Transform OriginPoint { get; }

        protected IWorldMember interactor;

        protected IInteractable interactable;
        private RaycastHit[] interactionRayHits;

        private bool isActive;

        public void Initialize(IWorldMember interactor)
        {
            this.interactor = interactor;
            
            tickableService.AddFixedUpdateTickable(new TickableEntity(CastInteractionRadius));
            interactionRayHits = new RaycastHit[32];
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }
        
        protected virtual void CastInteractionRadius()
        {
            Debug.DrawRay(OriginPoint.position, OriginPoint.forward * InteractionSetup.radius, Color.blue);

            if (!isActive)
            {
                UpdateInteractableRayCasted(null);
                return;
            }
            
            var hitsCount =
                Physics.SphereCastNonAlloc(
                    OriginPoint.position,
                    InteractionSetup.radius,
                    OriginPoint.forward,
                    interactionRayHits,
                    InteractionSetup.radius);

            if (hitsCount > 0)
            {
                for (var i = 0; i < hitsCount; i++)
                {
                    interactionRayHits[i].transform.TryGetComponent(out interactable);
                    if (interactable != null) break;
                }
            }
            else
            {
                UpdateInteractableRayCasted(null);
            }
            
            UpdateInteractableRayCasted(interactable);
        }

        protected virtual void UpdateInteractableRayCasted(IInteractable interactable)
        {
            this.interactable = interactable;
        }
    }
}