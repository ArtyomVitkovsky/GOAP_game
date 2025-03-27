using System.Collections.Generic;
using Game.NpcSystem;
using UnityEngine;

namespace AI.Action
{
    [CreateAssetMenu(menuName = "AI/Action/Generic/FindVehicleAction", fileName = "FindVehicleAction")]
    public class FindVehicleAction : ActorAction
    {
        [SerializeField] private int stopDistance;
        
        public override int GetCost(WorldState worldState)
        {
            worldState.GetClosestVehicle(out worldState.ClosestVehicle, out var distance);
            return (int) distance;
        }

        public override ActionPerformResult Perform(NpcCharacter npcCharacter, WorldState worldState)
        {
            worldState.GetClosestVehicle(out worldState.ClosestVehicle, out var distance);
            
            if (!npcCharacter.NavigateTo(worldState.ClosestVehicle, stopDistance))
            {
                return ActionPerformResult.Performing;
            }
            
            return npcCharacter.TryToInteractWithVehicle() ? ActionPerformResult.Completed : ActionPerformResult.Performing;
        }

        public override void Complete(NpcCharacter npcCharacter, WorldState worldState)
        {
            
        }

        public override void Fail(NpcCharacter npcCharacter, WorldState worldState)
        {
            worldState.SetEffect(WorldStateKeysEnum.IS_CAN_USE_VEHICLE, false);
        }
    }
}