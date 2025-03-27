using Game.Car.Installers;
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
        private Collider[] interactionRayHits;

        private bool isActive;

        public void Initialize(IWorldMember interactor)
        {
            this.interactor = interactor;
            
            tickableService.AddFixedUpdateTickable(new TickableEntity(CastInteractionRadius));
            interactionRayHits = new Collider[32];
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }
        
        protected virtual void CastInteractionRadius()
        {
            // DebugExtension.DrawDebugSphere(OriginPoint.position, InteractionSetup.radius, Color.green);

            if (!isActive)
            {
                UpdateInteractableRayCasted(null);
                return;
            }
            
            var hitsCount =
                Physics.OverlapSphereNonAlloc(
                    OriginPoint.position,
                    InteractionSetup.radius,
                    interactionRayHits,
                    InteractionSetup.layerMask);

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