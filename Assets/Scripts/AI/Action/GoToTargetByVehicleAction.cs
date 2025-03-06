using System.Collections.Generic;
using Game.NpcSystem;
using UnityEngine;

namespace AI.Action
{
    [CreateAssetMenu(menuName = "AI/Action/Generic/GoToTargetByVehicleAction", fileName = "GoToTargetByVehicleAction")]
    public class GoToTargetByVehicleAction : MovementAction
    {
        public override int GetCost(WorldState worldState)
        {
            var distance = (worldState.Position - worldState.Target).sqrMagnitude;

            if (distance > stopDistance)
            {
                return (int)(distance / costMultiplier);
            }

            return (int)(distance * costMultiplier);
        }

        public override ActionPerformResult Perform(NpcCharacter npcCharacter, WorldState worldState)
        {
            var isCompleted = false;
            var isOnTarget = npcCharacter.NavigateTo(worldState.Target, stopDistance);

            if (!npcCharacter.ControlVehicle())
            {
                return ActionPerformResult.Failed;
            }

            if (isOnTarget)
            {
                isCompleted = npcCharacter.StopVehicle();
            }

            return isCompleted ? ActionPerformResult.Completed : ActionPerformResult.Performing;
        }

        public override void Complete(NpcCharacter npcCharacter, WorldState worldState)
        {
            npcCharacter.LeaveVehicle();
        }

        public override void Fail(NpcCharacter npcCharacter, WorldState worldState)
        {
            npcCharacter.LeaveVehicle();
        }
    }
}