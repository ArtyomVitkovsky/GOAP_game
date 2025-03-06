using Game.Character.Installers;
using Services.InteractionService;
using UnityEngine;
using Zenject;

namespace Game.Character.Components
{
    public class CharacterInteractionComponentInstaller : Installer<CharacterInteractionComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CharacterInteractionComponent>().AsSingle().NonLazy();
        }
    }

    public class CharacterInteractionComponent : InteractionComponent
    {
        [Inject] private IInteractionService interactionService;

        [Inject(Id = CharacterSystemInstaller.CHARACTER_INTERACTION_SETUP)]
        private CharacterInteractionSetup interactionSetup;
        
        [Inject(Id = CharacterSystemInstaller.CHARACTER_INTERACTION_ORIGIN_POINT)]
        private Transform originPoint;

        protected override CharacterInteractionSetup InteractionSetup => interactionSetup;
        protected override Transform OriginPoint => originPoint;

        protected override void UpdateInteractableRayCasted(IInteractable interactable)
        {
            base.UpdateInteractableRayCasted(interactable);
            
            interactionService.TrySetInteractable(interactable);
        }
    }
}