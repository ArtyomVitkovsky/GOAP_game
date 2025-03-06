using System.Collections.Generic;
using System.Linq;
using Game.NpcSystem.Components;
using UnityEngine;

namespace Services.ReputationService
{
    [CreateAssetMenu(menuName = "Configs/Fractions/Reputation/ReputationSetup", fileName = "ReputationSetup")]
    public class ReputationSetup : ScriptableObject
    {
        [SerializeField] private List<ReputationToMood> reputationToMoodSetups;
    
        private void OnValidate()
        {
            var newOrder = reputationToMoodSetups.OrderBy(s => s.Reputation);
            reputationToMoodSetups = newOrder.ToList();
        }

        public NpcMood GetMood(int reputation)
        {
            var currentMoodSetup = reputationToMoodSetups[0];
            foreach (var reputationToMood in reputationToMoodSetups)
            {
                if (reputationToMood.Reputation > reputation)
                {
                    break;
                }

                currentMoodSetup = reputationToMood;
            }
        
            return currentMoodSetup.Mood;
        }
    }
}