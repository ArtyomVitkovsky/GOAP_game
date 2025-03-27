using System;
using System.Collections.Generic;
using AI.Action;
using AI.Goal;
using Cysharp.Threading.Tasks;
using Game.Car;
using Game.Character.Components;
using Game.NpcSystem.Components;
using Game.NpcSystem.Installers;
using Services.InteractionService;
using Services.NpcLifeService;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Game.NpcSystem
{
    public class NpcCharacter : MonoBehaviour, IWorldMember, IDamagable, IGoapContext
    {
        [Inject] private ITickableService tickableService;
        [Inject] private IActionPlaningService actionPlaningService;
    
        [Inject] private NpcEncounterComponent encounterComponent;
        [Inject] private NpcInteractionComponent interactionComponent;
        [Inject] private NpcMoodComponent moodComponent;
        [Inject] private NpcNavigationComponent navigationComponent;
        [Inject] private NpcHealthComponent healthComponent;
        [Inject] private NpcSensorsComponent sensorsComponent;
        // [Inject] private AgentComponent agentComponent;

        [Inject(Id = NpcCharacterInstaller.HEALTH_VALUE)] 
        private int health;

        private Transform parent;

        private GoapAgent goapAgent;
        
        public WorldState WorldState;
        public VehicleSystem Vehicle;

        public NpcNavigationComponent NavigationComponent => navigationComponent;
        
        public GoapAgent GoapAgent => goapAgent;

        public Transform Transform => transform;
        public string Name { get; set; }

        public Fraction Fraction => WorldState.Fraction;
        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }

        public void ResetParent()
        {
            transform.parent = parent;
        }

        public void Initialize(Fraction fraction)
        {
            parent = transform.parent;
            
            WorldState = new WorldState();
            WorldState.MaxHealth = WorldState.Health = health;
            WorldState.Fraction = fraction;

            navigationComponent.Initialize();
            interactionComponent.Initialize(this);
            interactionComponent.SetActive(true);
            
            sensorsComponent.Initialize(this,WorldState);
            healthComponent.Initialize(WorldState);
            
            goapAgent = new GoapAgent(this, WorldState, actionPlaningService);
            tickableService.AddFixedUpdateTickable(new TickableEntity(UpdateWorldStatePosition));
        }

        private void UpdateWorldStatePosition()
        {
            WorldState.Position = transform.position;
        }

        public void ReceiveDamage(IDamageDealer damageDealer)
        {
            healthComponent.UpdateHealthValue(-damageDealer.Damage);

            WorldState.SetEffect(WorldStateKeysEnum.IS_ENEMY_NEARBY, sensorsComponent.ScanFor(damageDealer.Sender));

            moodComponent.UpdateCurrentMood(NpcMood.Aggressive);
        
            if (WorldState.Health <= 0)
            {
                transform.localPosition = Vector3.zero;
            }
        }

        public bool NavigateTo(Vector3 target, float stopDistance)
        {
            return navigationComponent.NavigateTo(target, stopDistance);
        }
        
        public bool Heal()
        {
            var isHealed = healthComponent.Heal();
            navigationComponent.SetActive(isHealed);
            return isHealed;
        }

        public bool TryToInteractWithVehicle()
        {
            var isInteractionSuccessful = interactionComponent.TryToInteract(out Vehicle);
            
            WorldState.SetEffect(WorldStateKeysEnum.IS_HAS_VEHICLE, isInteractionSuccessful);

            return isInteractionSuccessful;
        }

        public void LeaveVehicle()
        {
            if (Vehicle == null)
            {
                return;
            }
            
            Vehicle.Exit();

            navigationComponent.NavMeshAgent.agentTypeID = AgentTypeID.GetAgentTypeIDByName("Humanoid");
            navigationComponent.NavMeshAgent.autoRepath = true;
            navigationComponent.NavMeshAgent.updateRotation = false;
            navigationComponent.NavMeshAgent.updatePosition = false;
            navigationComponent.NavMeshAgent.ResetPath();
            navigationComponent.NavMeshAgent.SetDestination(navigationComponent.Target);
            
            Vehicle = null;
            
            WorldState.SetEffect(WorldStateKeysEnum.IS_HAS_VEHICLE, false);
        }

        public bool StopVehicle()
        {
            return Vehicle.Stop();
        }

        public bool ControlVehicle()
        {
            if (Vehicle == null) return false;
            
            var isControlPossible = Vehicle.IsNpcControlPossible();
            navigationComponent.VehicleControl.Control();
            
            return isControlPossible;
        }
    }
}
