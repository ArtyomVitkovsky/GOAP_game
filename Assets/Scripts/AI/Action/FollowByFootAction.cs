using Game.NpcSystem;
using UnityEngine;

namespace AI.Action
{
    [CreateAssetMenu(menuName = "AI/Action/Generic/FollowByFootAction", fileName = "FollowByFootAction")]
    public class FollowByFootAction : FollowAction
    {
        public override int GetCost(WorldState worldState)
        {
            return (int) (worldState.Position - worldState.Target).sqrMagnitude;
        }

        public override ActionPerformResult Perform(NpcCharacter npcCharacter, WorldState worldState)
        {
            var isNearTarget = npcCharacter.NavigateTo(worldState.Target, stopDistance);

            return IsFollowBraked(worldState) ? ActionPerformResult.Failed : ActionPerformResult.Performing;
        }

        public override void Fail(NpcCharacter npcCharacter, WorldState worldState)
        {
            worldState.SetEffect(WorldStateKeysEnum.IS_HAS_LEADER, false);
        }
    }
}