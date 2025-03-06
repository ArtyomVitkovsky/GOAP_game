using System.Collections.Generic;

namespace AI.Goal.Generic
{
    public class FollowGoal : Goal
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