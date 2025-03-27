using System.Collections.Generic;
using UnityEngine;

namespace AI.Goal.Generic
{
    [CreateAssetMenu(menuName = "AI/Goal/FollowGoal", fileName = "FollowGoal")]
    public class FollowActorGoal : ActorGoal
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