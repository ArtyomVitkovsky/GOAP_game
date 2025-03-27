using TMPro;
using UnityEngine;
using Zenject;

namespace Game.NpcSystem
{
    public class NpcGoapAgentDebugComponent : MonoBehaviour
    {
        [SerializeField] private NpcCharacter npcCharacter;

        [SerializeField] private TMP_Text currentGoal;
        [SerializeField] private TMP_Text currentAction;

        private void Update()
        {
            if (npcCharacter == null || npcCharacter.GoapAgent == null) return;
            
            currentGoal.text = $"Goal : {npcCharacter.GoapAgent.CurrentGoal?.Id}";
            
            if (npcCharacter.GoapAgent.CurrentPlan != null && npcCharacter.GoapAgent.CurrentPlan.Count != 0)
            {
                currentAction.text = $"Action : {npcCharacter.GoapAgent.CurrentPlan.Peek().Id}";
            }
        }
    }
}