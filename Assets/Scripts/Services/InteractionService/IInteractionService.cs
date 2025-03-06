using System;
using Zenject;

namespace Services.InteractionService
{
    public class InteractionServiceInstaller : Installer<InteractionServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InteractionService>().AsSingle().NonLazy();
        }
    }

    public interface IInteractionService
    {
        public IInteractable CurrentInteractable { get; }
    
        public Action<IInteractable> OnCurrentInteractableChanged { get; set; }

        public void Bootstrap();

        public void TrySetInteractable(IInteractable interactable);
    }
}