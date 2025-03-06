using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Services.GameInputService
{
    public class GameInputServiceInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput playerInput;
        
        public override void InstallBindings()
        {
            Container.BindInstance(playerInput).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameInputService>().AsSingle().NonLazy();
        }
    }
}