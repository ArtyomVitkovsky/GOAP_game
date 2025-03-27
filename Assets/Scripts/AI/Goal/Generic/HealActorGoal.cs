using System.Collections.Generic;
using UnityEngine;

namespace AI.Goal.Generic
{
    [CreateAssetMenu(menuName = "AI/Goal/HealGoal", fileName = "HealGoal")]
    public class HealActorGoal : ActorGoal
    {
        public override int Priority(WorldState worldState)
        {
            var priority = (int)Mathf.Lerp(
                maximumPriority,
                minimumPriority,
                Mathf.Pow((float)worldState.Health / worldState.MaxHealth, 3)
            );
            
            return Mathf.Clamp(priority, minimumPriority, maximumPriority);
        }

        public override Dictionary<string, bool>  GetDesiredState()
        {
            return DesiredState;
        }
    }
}