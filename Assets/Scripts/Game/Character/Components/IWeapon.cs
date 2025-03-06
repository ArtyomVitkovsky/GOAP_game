namespace Game.Character.Components
{
    public interface IWeapon
    {
        public int Range { get; }

        public float AttackRate { get; }
    }
}