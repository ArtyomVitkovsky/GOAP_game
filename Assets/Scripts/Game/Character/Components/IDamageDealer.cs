using Game.NpcSystem;

namespace Game.Character.Components
{
    public interface IDamageDealer
    {
        public IWorldMember Sender { get; }
        public int Damage { get; }
    }
}