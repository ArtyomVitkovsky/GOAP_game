using UnityEngine;

namespace AI.Action
{
    public class FollowAction : MovementAction
    {
        [SerializeField] protected int followDistance;
        [SerializeField] protected int brakeFollowDistance;
        
        protected bool IsFollowBraked(WorldState worldState)
        {
            var isFollowBraked = (worldState.Position - worldState.Target).sqrMagnitude > brakeFollowDistance;
            return isFollowBraked;
        }
    }
}