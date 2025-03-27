using System.Collections.Generic;
using UnityEngine;

namespace AI.Goal
{
    [CreateAssetMenu(menuName = "AI/Goal/GoalsSet", fileName = "GoalsSet")]
    public class GoalsSet : ScriptableObject
    {
        [SerializeField] private ActorGoal[] baseGoals;
        [SerializeField] private ActorGoal[] subordinateGoals;

        public ActorGoal[] BaseGoals => baseGoals;
        public ActorGoal[] SubordinateGoals => subordinateGoals;
    }
}