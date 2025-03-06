using System.Collections.Generic;
using Game.NpcSystem;
using UnityEngine;

namespace AI.Action
{
    [CreateAssetMenu(menuName = "AI/Action/Generic/HealAction", fileName = "HealAction")]
    public class HealAction : ActorAction
    {
        public override int GetCost(WorldState worldState)
        {
            return (int) Mathf.Lerp(maximumCost, minimumCost, Mathf.Pow(worldState.Health / 100f, 3));
        }

        public override ActionPerformResult Perform(NpcCharacter npcCharacter, WorldState worldState)
        {
            return npcCharacter.Heal() ? ActionPerformResult.Completed : ActionPerformResult.Performing;
        }

        public override void Complete(NpcCharacter npcCharacter, WorldState worldState)
        {
            worldState.SetEffect(WorldStateKeys.IS_DAMAGED, worldState.Health < worldState.MaxHealth);
        }

        public override void Fail(NpcCharacter npcCharacter, WorldState worldState)
        {
            worldState.SetEffect(WorldStateKeys.IS_DAMAGED, worldState.Health < worldState.MaxHealth);
        }
    }
}