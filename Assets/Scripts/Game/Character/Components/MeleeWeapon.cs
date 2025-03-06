namespace Game.Character.Components
{
    public class MeleeWeapon : IWeapon
    {
        public IDamageDealer DamageDealer { get; }
        
        public int Range { get; }
        public float AttackRate { get; }
    }
}