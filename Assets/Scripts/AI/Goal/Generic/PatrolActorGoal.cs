using System.Collections.Generic;
using UnityEngine;

namespace AI.Goal.Generic
{
    [CreateAssetMenu(menuName = "AI/Goal/PatrolGoal", fileName = "PatrolGoal")]
    public class PatrolActorGoal : ActorGoal
    {
        public override int Priority(WorldState worldState)
        {
            return defaultPriority;
        }

        public override Dictionary<string, bool> GetDesiredState()
        {
            return DesiredState;
        }
    }
}