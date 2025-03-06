namespace Game.Character.Components
{
    public interface ICombatService
    {
        public void ProcessAttack(IDamageDealer weapon, IDamagable damagable);
    }
}