using System;
using Game.CameraSystem.Installers;
using Game.NpcSystem;
using Services.TickableService;
using Services.TurretsService;
using UnityEngine;
using Zenject;

namespace Game.Turret
{
    public class Turret : MonoBehaviour, ITurret
    {
        [Inject] private DiContainer diContainer;
        [Inject] private ITickableService tickableService;
        
        [SerializeField] private TurretView view;

        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform turretPointer;
        
        [SerializeField] private Transform lookTarget;

        private IWorldMember owner;

        private TickableEntity tickableEntity;

        public Transform TurretPointer => turretPointer;

        private void Awake()
        {
            view.SetPointer(TurretPointer);
            tickableEntity ??= new TickableEntity(LookAtTarget);
        }

        public void SetOwner(IWorldMember owner)
        {
            this.owner = owner;
        }

        public void UpdateLookTarget(Transform lookTarget)
        {
            this.lookTarget = lookTarget;
        }

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                tickableEntity ??= new TickableEntity(LookAtTarget);
                tickableService.AddUpdateTickable(tickableEntity);
            }
            else
            {
                tickableService.RemoveUpdateTickable(tickableEntity);
                view.ResetRotation();
            }
        }

        public void LookAtTarget()
        {
            view.LookAtTarget(lookTarget);
        }

        public void Shoot()
        {
            var extraArgs = new[] { owner };
            var projectile = diContainer.InstantiatePrefabForComponent<Projectile>(projectilePrefab, extraArgs);
            projectile.transform.position = view.ProjectileAnchor.position;
                
            projectile.transform.LookAt(TurretPointer);
            projectile.Launch();
        }
    }
}
