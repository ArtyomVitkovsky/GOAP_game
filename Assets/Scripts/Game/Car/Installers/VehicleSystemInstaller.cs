using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Car.Installers
{
    public enum DirectionType
    {
        Forward = 0,
        ForwardRight = 1,
        ForwardLeft = 2,
        Back = 3,
        BackRight = 4,
        BackLeft = 5,
        Right = 6,
        Left = 7
    }

    [Serializable]
    public class ObstacleCheckerOrigin
    {
        public DirectionType direction;
        public Transform[] origins;
    }
    
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
        [SerializeField] private ObstacleCheckerOrigin[] obstacleTriggerOrigin;
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

            var obstacleOrigins = 
                obstacleTriggerOrigin.ToDictionary(v => v.direction, v => v.origins);
            Container.BindInstance(obstacleOrigins).WithId(VEHICLE_OBSTACLE_TRIGGER_ORIGIN);
            
            VehicleTurretsComponentInstaller.Install(Container);
            PlayerVehicleControlComponentInstaller.Install(Container);
            NpcVehicleControlComponentInstaller.Install(Container);
            VehicleTransmissionComponentInstaller.Install(Container);
            VehicleCharacterPositionsComponentInstaller.Install(Container);
        }
    }
}
