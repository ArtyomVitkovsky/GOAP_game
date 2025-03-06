using System;
using Game.Character;
using Game.Character.Components;
using Services.GameInputService;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Services.PlayerControlService
{
    public class PlayerControlService : IPlayerControlService
    {
        [Inject] private ITickableService tickableService;
        [Inject] private IGameInputService gameInputService;

        private Transform defaultCharacterParent;
        
        public IControllable CharacterControllable { get; private set; }
        public IControllable CurrentControllable { get; private set; }
        
        public Action<IControllable> OnControllableChange { get; set; }

        public PlayerActionsProvider PlayerActionsProvider { get; private set; }
        public VehicleActionsProvider VehicleActionsProvider { get; private set; }

        public void Bootstrap()
        {
            PlayerActionsProvider = new PlayerActionsProvider();
            VehicleActionsProvider = new VehicleActionsProvider();
            
            gameInputService.AddPlayerCallbacks(PlayerActionsProvider);
            gameInputService.AddVehicleCallbacks(VehicleActionsProvider);
            
            tickableService.AddFixedUpdateTickable(new TickableEntity(Control));
        }

        public void SetCharacterControllable(IControllable controllable)
        {
            CharacterControllable = controllable;
            defaultCharacterParent = controllable.ControllableTransform.parent;
        }

        public void SetCurrentControllable(IControllable controllable)
        {
            if (CurrentControllable == controllable) return;
            
            CurrentControllable?.SetActive(false);

            CurrentControllable = controllable;
            CurrentControllable.SetActive(true);

            OnControllableChange?.Invoke(CurrentControllable);
        }

        public void ResetCurrentControllable()
        {
            SetCurrentControllable(CharacterControllable);
        }

        public void SetCharacterParent(Transform parent)
        {
            CharacterControllable.ControllableTransform.parent = parent;
        }

        public void ResetCharacterParent()
        {
            CharacterControllable.ControllableTransform.parent = defaultCharacterParent;
        }

        public void Control()
        {
            CurrentControllable?.Control();
        }
    }
}