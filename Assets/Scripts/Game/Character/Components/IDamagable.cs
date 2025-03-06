namespace Game.Character.Components
{
    public interface IDamagable
    {
        public void ReceiveDamage(IDamageDealer damageDealer);
    }
}