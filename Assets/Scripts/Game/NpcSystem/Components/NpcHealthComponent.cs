using Game.NpcSystem.Installers;
using Services.PlayerControlService;
using Services.TickableService;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.NpcSystem.Components
{
    public class NpcHealthComponent
    {
        [Inject] ITickableService tickableService;
        [Inject] IPlayerControlService playerControlService;
    
        [Inject(Id = NpcCharacterInstaller.HEALTH_TEXT)]
        private TMP_Text healthText;

        private WorldState WorldState;
        private float healedHealth;

        public void Initialize(WorldState worldState)
        {
            WorldState = worldState;
            tickableService.AddFixedUpdateTickable(new TickableEntity(HealthLookAtPlayer));
        }

        public void UpdateHealthValue(int difference)
        {
            WorldState.Health += difference;
            
            healthText.SetText($"{WorldState.Health} / {WorldState.MaxHealth}");

            WorldState.SetEffect(WorldStateKeysEnum.IS_DAMAGED, WorldState.Health < WorldState.MaxHealth);
        }

        private void HealthLookAtPlayer()
        {
            healthText.transform.LookAt(playerControlService.CharacterControllable.ControllableTransform);
        }

        public bool Heal()
        {
            healedHealth += Time.deltaTime;

            if (healedHealth > 1)
            {
                UpdateHealthValue((int) healedHealth);
                healedHealth = 0;
            }
            
            var isHealed = WorldState.Health == 100;
            healthText.color = isHealed ? Color.white : Color.green;
            
            return isHealed;
        }

        public void Dispose()
        {
            tickableService.RemoveFixedUpdateTickable(new TickableEntity(HealthLookAtPlayer));
        }
    }
}