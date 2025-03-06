using Game.NpcSystem;
using UnityEngine;

namespace AI.Action
{
    public class MovementAction : ActorAction
    {
        [SerializeField] protected int stopDistance;

        public override int GetCost(WorldState worldState)
        {
            return 1;
        }

        public override ActionPerformResult Perform(NpcCharacter npcCharacter, WorldState worldState)
        {
            return ActionPerformResult.Performing;
        }

        public override void Complete(NpcCharacter npcCharacter, WorldState worldState)
        {
        }

        public override void Fail(NpcCharacter npcCharacter, WorldState worldState)
        {
        }
    }
}