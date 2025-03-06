using System.Collections.Generic;
using Services.ReputationService;
using Zenject;

namespace Game.NpcSystem.Components
{
    public class NpcEncounterComponent
    {
        private static readonly Dictionary<NpcMood, NpcInteractionType> MoodToInteraction =
            new Dictionary<NpcMood, NpcInteractionType>()
            {
                { NpcMood.Aggressive, NpcInteractionType.Attack},
                { NpcMood.Neutral, NpcInteractionType.Trade},
                { NpcMood.Normal, NpcInteractionType.Trade},
                { NpcMood.Good, NpcInteractionType.Trade},
                { NpcMood.Friendly, NpcInteractionType.Trade}
            };
    
        [Inject] private IReputationService reputationService;
        [Inject] private NpcMoodComponent npcMoodComponent;

        public IWorldMember npc;
        public IWorldMember CurrentInteractionSender { get; private set; }
        public IWorldMember CurrentInteractionTarget { get; private set; }

        public void ProcessInteractionRequest(NpcInteractionRequest request)
        {
            CurrentInteractionSender = request.Sender;
        
            switch (request.Type)
            {
                case NpcInteractionType.Trade:
                {
                    break;
                }
                case NpcInteractionType.Attack:
                {
                    npcMoodComponent.UpdateCurrentMood(NpcMood.Aggressive);
                    break;
                }
            }
        }

        public void TryToProceedInteraction(IWorldMember interactionTarget)
        {
            CurrentInteractionTarget = interactionTarget;

            var interactionMood = reputationService.GetMood(npc, interactionTarget);

            if (MoodToInteraction.TryGetValue(interactionMood, out var interactionType))
            {
                TrySendInteractionRequest(npc, interactionType, interactionTarget);
            }
        }

        private void TrySendInteractionRequest(IWorldMember sender, NpcInteractionType type, IWorldMember receiver)
        {
        
        }
    }
}