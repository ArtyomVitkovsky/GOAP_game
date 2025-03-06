using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.NpcSystem
{
    [CreateAssetMenu(menuName = "Configs/Fractions/Reputation/FractionReputationSetupSet", fileName = "FractionReputationSetupSet")]
    public class FractionReputationSetupSet : ScriptableObject
    {
        [SerializeField] private List<FractionReputationSetup> fractionReputationSetups;

        public int GetReputation(Fraction from, Fraction to)
        {
            var fractionReputationSetup = 
                fractionReputationSetups.FirstOrDefault(f => f.From.Fraction.Name == from.Name);

            return fractionReputationSetup.GetReputation(to);
        }
    }
}