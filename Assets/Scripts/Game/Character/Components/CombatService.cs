namespace Game.Character.Components
{
    public class CombatService : ICombatService
    {
        public void ProcessAttack(IDamageDealer damageDealer, IDamagable damagable)
        {
            damagable.ReceiveDamage(damageDealer);
        }
    }
}