using Cysharp.Threading.Tasks;
using Game.Car;
using Services;
using UI.MainScreen.Components;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainScreen
{
    public class MainScreen : MonoBehaviour
    {
        [Inject] private IBootstrapService bootstrapService;
    
        [Inject] private SignalBus signalBus;
        
        [Inject] private CrosshairComponent crosshairComponent;
        [Inject] private InteractionUserInterfaceComponent interactionUserInterface;
        
        private void Awake()
        {
            Initialize();
        }

        private async UniTask Initialize()
        {
            await bootstrapService.BootstrapTask;
            
            crosshairComponent.Initialize();
            interactionUserInterface.Initialize();
        }
    }
}
