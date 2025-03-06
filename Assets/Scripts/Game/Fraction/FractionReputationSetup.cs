using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.NpcSystem
{
    [CreateAssetMenu(menuName = "Configs/Fractions/Reputation/FractionReputationSetup", fileName = "FractionReputationSetup")]
    public class FractionReputationSetup : ScriptableObject
    {
        [SerializeField] private FractionSetup from;
        [SerializeField] private List<FractionRelationSetup> relationSetups;
    
        public FractionSetup From => from;

        public int GetReputation(Fraction fraction)
        {
            var relationSetup = 
                relationSetups.FirstOrDefault(s => s.FractionSetup.Fraction.Name == fraction.Name);
        
            return relationSetup.Reputation;
        }
    }
}