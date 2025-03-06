namespace Game.Character.Components
{
    public class FirearmWeapon : IWeapon
    {
        public IDamageDealer DamageDealer { get; }

        public int Range { get; }
        public float AttackRate { get; }
    }
}