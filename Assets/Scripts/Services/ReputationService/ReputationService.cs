using Game.NpcSystem;
using Game.NpcSystem.Components;
using Zenject;

namespace Services.ReputationService
{
    public class ReputationService : IReputationService
    {
        [Inject] private ReputationSetup reputationSetup;
        [Inject] private FractionReputationSetupSet fractionReputationSetupSet;
    
        public NpcMood GetMood(IWorldMember from, IWorldMember to)
        {
            var reputation = fractionReputationSetupSet.GetReputation(from.Fraction, to.Fraction);

            return reputationSetup.GetMood(reputation);
        }

        public void ProceedReputationUpdateFor(IWorldMember actor, IWorldMember target)
        {
        
        }
    }
}