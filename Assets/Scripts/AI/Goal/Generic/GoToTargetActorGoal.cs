using System.Collections.Generic;
using AI.Action;
using UnityEngine;

namespace AI.Goal.Generic
{
    [CreateAssetMenu(menuName = "AI/Goal/GoToTargetGoal", fileName = "GoToTargetGoal")]
    public class GoToTargetActorGoal : ActorGoal
    {
        public override int Priority(WorldState worldState)
        {
            return defaultPriority;
        }

        public override Dictionary<string, bool>  GetDesiredState()
        {
            return DesiredState;
        }
    }
}