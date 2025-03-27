using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Car.Installers
{
    [ExecuteAlways]
    public class ObstacleTriggerDebug : MonoBehaviour
    {
        [SerializeField] protected VehicleMovementBaseStats _vehicleMovementBaseStats;
        
        [SerializeField] private ObstacleCheckerOrigin[] obstacleTriggerOrigin;
        
        private Collider[] obstacles = new Collider[1];
        
        private void Update()
        {
            CheckObstacles();
        }
        
        private void CheckObstacles()
        {
            if (_vehicleMovementBaseStats == null) return;
            if (obstacleTriggerOrigin == null) return;
            
            CheckObstacle(DirectionType.Forward);
            CheckObstacle(DirectionType.ForwardRight);
            CheckObstacle(DirectionType.ForwardLeft);
            CheckObstacle(DirectionType.Back);
            CheckObstacle(DirectionType.BackRight);
            CheckObstacle(DirectionType.BackLeft);
            CheckObstacle(DirectionType.Right);
            CheckObstacle(DirectionType.Left);
        }
        
        private void CheckObstacle(DirectionType direction)
        {
            var distance = _vehicleMovementBaseStats.ObstacleDistance(direction);
            var obstacleChecker = obstacleTriggerOrigin.FirstOrDefault(o=>o.direction == direction);

            if (obstacleChecker == null || obstacleChecker.origins == null) return;

            var obstacleExists = false;
            for (int i = 0; i < obstacleChecker.origins.Length; i++)
            {
                var size = Physics.OverlapSphereNonAlloc(
                    obstacleChecker.origins[i].position, distance, obstacles, ~_vehicleMovementBaseStats.ExcludeObstacleLayerMask);
                
                obstacleExists = size > 0;
                
                var color = obstacleExists ? Color.red : Color.green;
                Debug.DrawRay(obstacleChecker.origins[i].position, obstacleChecker.origins[i].forward * distance, color);
                if (obstacleExists)
                {
                    Debug.DrawLine(
                        obstacles[0].transform.position,
                        obstacles[0].transform.position + Vector3.up * 0.5f,
                        Color.yellow);
                }

                DebugExtension.DrawDebugSphere(obstacleChecker.origins[i].position, distance, color);
            }
        }
    }
}