using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Car.Installers
{
    [Serializable]
    public class ObstacleSensorDistance
    {
        public DirectionType direction;
        public float distance;
    }
    
    [CreateAssetMenu(menuName = "Configs/Player/Movement/VehicleMovement", fileName = "VehicleMovementBaseStats")]
    public class VehicleMovementBaseStats : ScriptableObject
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float maxSteerAngle;
        [SerializeField] private float stopBrakeTorque;
        [SerializeField] private float brakeTorque;
        
        [Header("Auto Control")]
        [SerializeField] private float steerAdjustment;
        [SerializeField] private float reverseTargetAngle;
        [SerializeField] private LayerMask excludeObstacleLayerMask;
        [SerializeField] private ObstacleSensorDistance[] obstacleDistance;
        
        public float MoveSpeed => moveSpeed;
        public float RotationSpeed => rotationSpeed;
        public float MaxSteerAngle => maxSteerAngle;
        public float StopBrakeTorque => stopBrakeTorque;
        public float BrakeTorque => brakeTorque;
        public float SteerAdjustment => steerAdjustment;
        public float ReverseTargetAngle => reverseTargetAngle;
        public LayerMask ExcludeObstacleLayerMask => excludeObstacleLayerMask;
        
        public float ObstacleDistance(DirectionType direction)
        {
            var obstacleDistanceSetup = obstacleDistance.FirstOrDefault(o=>o.direction == direction);
            
            return obstacleDistanceSetup == null ? 0 : obstacleDistanceSetup.distance;
        }
    }
}