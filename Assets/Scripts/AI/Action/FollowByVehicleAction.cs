using Game.NpcSystem;
using UnityEngine;

namespace AI.Action
{
    [CreateAssetMenu(menuName = "AI/Action/Generic/FollowByVehicleAction", fileName = "FollowByVehicleAction")]
    public class FollowByVehicleAction : FollowAction
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
            npcCharacter.NavigateTo(worldState.Target, followDistance);

            if (!npcCharacter.ControlVehicle())
            {
                return ActionPerformResult.Failed;
            }

            var isOnTarget = (worldState.Position - worldState.Target).sqrMagnitude < stopDistance;
            if (isOnTarget)
            {
                npcCharacter.StopVehicle();
            }

            return IsFollowBraked(worldState) ? ActionPerformResult.Failed : ActionPerformResult.Performing;
        }
    }
}