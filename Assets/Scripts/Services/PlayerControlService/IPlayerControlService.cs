using System;
using Game.Character.Components;
using UnityEngine;

namespace Services.PlayerControlService
{
    public interface IPlayerControlService
    {
        public IControllable CharacterControllable { get; }
        public IControllable CurrentControllable { get; }

        public Action<IControllable> OnControllableChange { get; set; }

        public PlayerActionsProvider PlayerActionsProvider { get; }
        public VehicleActionsProvider VehicleActionsProvider { get; }
        
        public void Bootstrap();

        public void SetCharacterControllable(IControllable controllable);
        public void SetCurrentControllable(IControllable controllable);
        public void ResetCurrentControllable();

        public void SetCharacterParent(Transform parent);
        
        public void ResetCharacterParent();

        public void Control();
    }
}
