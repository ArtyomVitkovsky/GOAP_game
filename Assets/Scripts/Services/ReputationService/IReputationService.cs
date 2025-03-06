using Game.NpcSystem;
using Game.NpcSystem.Components;

namespace Services.ReputationService
{
    public interface IReputationService
    {
        public NpcMood GetMood(IWorldMember from, IWorldMember to);

        public void ProceedReputationUpdateFor(IWorldMember actor, IWorldMember target);
    }
}