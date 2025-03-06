using System.Collections.Generic;
using UnityEngine;

namespace AI.Goal
{
    [CreateAssetMenu(menuName = "AI/Goal/GoalsSet", fileName = "GoalsSet")]
    public class GoalsSet : ScriptableObject
    {
        [SerializeField] private Goal[] baseGoals;
        [SerializeField] private Goal[] subordinateGoals;

        public Goal[] BaseGoals => baseGoals;
        public Goal[] SubordinateGoals => subordinateGoals;
    }
}