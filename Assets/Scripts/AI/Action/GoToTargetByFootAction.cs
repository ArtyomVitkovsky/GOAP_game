using System.Collections.Generic;
using Game.NpcSystem;
using UnityEngine;

namespace AI.Action
{
    [CreateAssetMenu(menuName = "AI/Action/Generic/GoToTargetByFootAction", fileName = "GoToTargetByFootAction")]

    public class GoToTargetByFootAction : MovementAction
    {
        public override int GetCost(WorldState worldState)
        {
            return (int) (worldState.Position - worldState.Target).sqrMagnitude;
        }

        public override ActionPerformResult Perform(NpcCharacter npcCharacter, WorldState worldState)
        {
            return npcCharacter.NavigateTo(worldState.Target, stopDistance)
                ? ActionPerformResult.Performing
                : ActionPerformResult.Completed;
        }

        public override void Complete(NpcCharacter npcCharacter, WorldState worldState)
        {
            worldState.SetEffect(WorldStateKeys.IS_AT_TARGET, true);
            worldState.SetEffect(WorldStateKeys.IS_HAS_TARGET, false);
        }

        public override void Fail(NpcCharacter npcCharacter, WorldState worldState)
        {
        }
    }
}