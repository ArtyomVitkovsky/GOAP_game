using System.Collections.Generic;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Services.TurretsService
{
    public class PlayerTurretsService : IPlayerTurretsService
    {
        [Inject] private ITickableService tickableService;
    
        private List<ITurret> turrets;

        public Transform TurretsPointer { get; private set; }

        public void Bootstrap()
        {
            turrets = new List<ITurret>();
        }

        public void SetTurretsPointer(Transform pointer)
        {
            TurretsPointer = pointer;
        }

        public void RemoveTurretsPointer()
        {
            TurretsPointer = null;
        }

        public void AddTurret(ITurret turret)
        {
            turrets ??= new List<ITurret>();
            turrets.Add(turret);
        }

        public void RemoveTurret(ITurret turret)
        {
            turrets.Remove(turret);
        }
    }
}