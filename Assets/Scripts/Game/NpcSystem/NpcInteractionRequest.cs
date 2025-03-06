namespace Game.NpcSystem
{
    public enum NpcInteractionType
    {
        Trade = 0,
        Attack = 1
    }

    public class NpcInteractionRequest
    {
        public IWorldMember Sender;
        public NpcInteractionType Type;
    
        public NpcInteractionRequest(IWorldMember sender, NpcInteractionType type)
        {
            Sender = sender;
            Type = type;
        }
    }
}