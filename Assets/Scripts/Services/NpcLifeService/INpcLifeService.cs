using Game.NpcSystem;

namespace Services.NpcLifeService
{
    public interface INpcLifeService
    {
        public void Bootstrap();
        public void RegisterCampPoint(NpcCampPoint campPoint);
        public void RegisterInterestPoint(IInterestPoint interestPoint);
    }
}