using System.Collections.Generic;
using System.Linq;
using Game.Car.Installers;
using Game.NpcSystem;
using Services.PlayerControlService;
using UnityEngine;
using Zenject;

namespace Game.Car.Components
{
    public class VehicleCharacterPositionsComponent
    {
        [Inject(Id = VehicleSystemInstaller.VEHICLE_CHARACTER_POSITIONS)]
        private List<VehicleCharacterPosition> positions;

        public void SetCharacterTo(IWorldMember owner, VehicleCharacterPositionType type)
        {
            var characterPosition = positions.FirstOrDefault(p => p.Type == type);
            
            if (characterPosition == null) return;

            owner.SetParent(characterPosition.Transform);
            owner.Transform.position = characterPosition.Transform.position;
            owner.Transform.localRotation = Quaternion.Euler(Vector3.zero);
            
            if (type == VehicleCharacterPositionType.EnterExit)
            {
                owner.ResetParent();
            }
        }
    }
}