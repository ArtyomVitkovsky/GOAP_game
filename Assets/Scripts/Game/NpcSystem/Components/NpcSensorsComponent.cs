using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AI.Sensors;
using Cysharp.Threading.Tasks;
using Game.Car;
using UnityEngine;
using Zenject;

namespace Game.NpcSystem.Components
{
    public class NpcSensorsComponent
    {
        [Inject] private List<BaseSensor<RaycastHit>> rayCastSensors;

        private IWorldMember Npc;
        
        private WorldState WorldState;

        private List<RaycastHit> RaycastHits;

        private CancellationTokenSource cts;
        
        public void Initialize(IWorldMember npc, WorldState worldState)
        {
            Npc = npc;
            WorldState = worldState;

            StartPeriodicalScanning();
        }

        private async UniTask StartPeriodicalScanning()
        {
            cts = new CancellationTokenSource();

            while (!cts.IsCancellationRequested)
            {
                await UniTask.Delay(1000)
                    .AttachExternalCancellation(cts.Token)
                    .SuppressCancellationThrow();
                
                if (cts.IsCancellationRequested) return;
                
                Scan();
            }
        }

        public void Scan()
        {
            RaycastHits?.Clear();
            RaycastHits = new List<RaycastHit>(32 * rayCastSensors.Count);
            foreach (var sensor in rayCastSensors)
            {
                var hits = sensor.Invoke().ToList().FindAll(hit => hit.transform != null);
                RaycastHits.AddRange(hits);
            }

            if (RaycastHits.Count > 0)
            {
                UpdateWorldState();
            }
        }

        public bool ScanFor(IWorldMember target)
        {
            Scan();

            for (int i = 0; i < RaycastHits.Count; i++)
            {
                var hit = RaycastHits[i];
                if (hit.transform.TryGetComponent(out IWorldMember worldMember) && worldMember == target)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateWorldState()
        {
            UpdateVehiclesInformation();
        }

        private void UpdateVehiclesInformation()
        {
            try
            {
                var capacity = RaycastHits.Count(
                    h => h.transform.TryGetComponent(out VehicleSystem vehicle)
                );
                
                if (capacity == 0) return;

                var vehicles = new List<VehicleSystem>(capacity);
                
                RaycastHits.ForEach(h =>
                {
                    if (h.transform.TryGetComponent(out VehicleSystem vehicle))
                    {
                        if (!vehicles.Contains(vehicle) && vehicle.Owner == null || vehicle.Owner == Npc)
                        {
                            if (vehicle.IsNpcControlPossible())
                            {
                                vehicles.Add(vehicle);
                            }
                        }
                    }
                });

                if (vehicles.Count == 1 && vehicles[0].Owner == Npc)
                {
                    return;
                } 
                
                WorldState.VehiclePositions?.Clear();
                WorldState.VehiclePositions = vehicles.Select(v => v.transform.position).ToList();
                WorldState.SetEffect(WorldStateKeysEnum.IS_CAN_USE_VEHICLE, vehicles.Count > 0);
                WorldState.SetEffect(WorldStateKeysEnum.IS_SEE_VEHICLE, true);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
        }
    }
}