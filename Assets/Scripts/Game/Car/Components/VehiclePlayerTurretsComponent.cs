using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.CameraSystem.Installers;
using Game.Car.Installers;
using Game.NpcSystem;
using Services;
using Services.PlayerControlService;
using Services.TickableService;
using Services.TurretsService;
using UnityEngine;
using Zenject;

namespace Game.Car.Components
{
    public class VehicleTurretsComponent
    {
        [Inject] protected IBootstrapService bootstrapService;
        [Inject] protected ITickableService tickableService;
        
        [Inject(Id = VehicleSystemInstaller.VEHICLE_TURRETS)] 
        protected List<Turret.Turret> turrets;
        
        [Inject(Id = VehicleSystemInstaller.TURRETS_POINTER)]
        protected Transform turretsPointer;

        private IWorldMember owner;

        public virtual void Initialize(IWorldMember owner)
        {
            this.owner = owner;
            foreach (var turret in turrets)
            {
                turret.SetOwner(owner);
            }
        }

        public void SetTurretsLookTarget(Transform lookTarget)
        {
            foreach (var turret in turrets)
            {
                turret.UpdateLookTarget(lookTarget);
            }
        }

        public void SetTurretsActive(bool isActive)
        {
            foreach (var turret in turrets)
            {
                turret.SetActive(isActive);
            }
        }
    }
    
    public class VehiclePlayerTurretsComponent : VehicleTurretsComponent
    {
        [Inject] private IPlayerControlService _playerControlService;
        [Inject] private IPlayerTurretsService _playerTurretsService;
        
        [Inject(Id = CameraSystemInstaller.CAMERA_RAYCAST_POINTER)]
        private Transform pointer;

        private float fireRate = 100;
        private float currentFireTimeOut;

        private VehicleActionsProvider VehicleActionsProvider => _playerControlService.VehicleActionsProvider;
        
        public override void Initialize(IWorldMember owner)
        {
            base.Initialize(owner);
            
            tickableService.AddUpdateTickable(new TickableEntity(CalculateTurretsPointerPosition));
            tickableService.AddUpdateTickable(new TickableEntity(ProcessAttack));
            SetTurretsLookTarget(pointer);
            SetTurretsActive(true);
            _playerTurretsService.SetTurretsPointer(turretsPointer);
        }

        private void ProcessAttack()
        {
            if (currentFireTimeOut > 0)
            {
                currentFireTimeOut -= Time.deltaTime;
            }
            else if (VehicleActionsProvider.Attack)
            {
                foreach (var turret in turrets)
                {
                    turret.Shoot();
                }

                currentFireTimeOut = 60f / fireRate;
            }
        }
        
        private void CalculateTurretsPointerPosition()
        {
            var averagePosition = new Vector3();

            foreach (var turret in turrets)
            {
                averagePosition += turret.TurretPointer.position;
            }

            averagePosition = new Vector3(
                averagePosition.x / turrets.Count, 
                averagePosition.y / turrets.Count,
                averagePosition.z / turrets.Count);

            turretsPointer.position = averagePosition;
        }
    }
}