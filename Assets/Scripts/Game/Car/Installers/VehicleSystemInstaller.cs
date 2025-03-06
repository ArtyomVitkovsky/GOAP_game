using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Car.Installers
{
    public class VehicleSystemInstaller : MonoInstaller
    {
        public const string TURRETS_POINTER = "TURRETS_POINTER";
        public const string VEHICLE_RIGIDBODY = "VEHICLE_RIGIDBODY";
        public const string VEHICLE_WHEELS = "VEHICLE_WHEELS";
        public const string VEHICLE_TRANSFORM = "VEHICLE_TRANSFORM";
        public const string VEHICLE_TURRETS = "VEHICLE_TURRETS";
        public const string VEHICLE_CHARACTER_POSITIONS = "VEHICLE_CHARACTER_POSITIONS";
        public const string VEHICLE_OBSTACLE_TRIGGER_ORIGIN = "VEHICLE_OBSTACLE_TRIGGER_ORIGIN";

        // [SerializeField] private NavMeshAgent NavMeshAgent;
        
        [Header("Vehicle")]
        [SerializeField] private VehicleSystem vehicleSystem;
        [SerializeField] private Transform transform;
        [SerializeField] private Transform obstacleTriggerOrigin;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private List<Wheel> wheels;
        [SerializeField] private List<VehicleCharacterPosition> positions;
        
        [Header("Turrets")]
        [SerializeField] private Transform turretsPointer;
        [SerializeField] private List<Turret.Turret> turrets;
        

        public override void InstallBindings()
        {
            // Container.BindInstance(NavMeshAgent);
            Container.BindInstance(vehicleSystem);
            
            Container.BindInstance(turretsPointer).WithId(TURRETS_POINTER);
            Container.BindInstance(rigidbody).WithId(VEHICLE_RIGIDBODY);
            Container.BindInstance(wheels).WithId(VEHICLE_WHEELS);
            Container.BindInstance(transform).WithId(VEHICLE_TRANSFORM);
            Container.BindInstance(turrets).WithId(VEHICLE_TURRETS);
            Container.BindInstance(positions).WithId(VEHICLE_CHARACTER_POSITIONS);
            Container.BindInstance(obstacleTriggerOrigin).WithId(VEHICLE_OBSTACLE_TRIGGER_ORIGIN);
            
            VehicleTurretsComponentInstaller.Install(Container);
            PlayerVehicleControlComponentInstaller.Install(Container);
            NpcVehicleControlComponentInstaller.Install(Container);
            VehicleTransmissionComponentInstaller.Install(Container);
            VehicleCharacterPositionsComponentInstaller.Install(Container);
        }
    }
}
