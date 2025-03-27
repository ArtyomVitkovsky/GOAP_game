using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Game.Car.Installers;
using Game.NpcSystem;
using Game.NpcSystem.Components;
using Services.TickableService;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Zenject;
using Vector3 = UnityEngine.Vector3;

namespace Game.Car.Components
{
    public class NpcVehicleControlComponent : VehicleControlComponent
    {
        [Inject(Id = VehicleSystemInstaller.VEHICLE_OBSTACLE_TRIGGER_ORIGIN)]
        private Dictionary<DirectionType, Transform[]> obstacleTriggerOrigin;
        
        private Dictionary<DirectionType, bool> hasObstacle = new Dictionary<DirectionType, bool>();
        private Collider[] obstacles = new Collider[1];

        private float startDistance;
        private bool isReversing;
        private NpcNavigationComponent navigation;
        private NavMeshAgent agent;

        private Vector3 startPosition;

        private float steerCorrection;

        protected override bool IsInitialized { get; set; }

        public override void Initialize()
        {
            ReleaseBreaks();
            IsInitialized = true;
        }

        public override void Dispose()
        {
            Stop();
        }

        public void Setup(NpcNavigationComponent navigationComponent)
        {
            navigation = navigationComponent;
            
            agent = navigationComponent.NavMeshAgent;
            agent.agentTypeID = AgentTypeID.GetAgentTypeIDByName("Vehicle");
            agent.autoRepath = true;
            agent.updateRotation = false;
            agent.updatePosition = false;
            agent.ResetPath();
            agent.SetDestination(navigationComponent.Target);
            
            startDistance = 0;
            startPosition = rigidbody.transform.position;
        }

        public override void Control()
        {
            var transform = rigidbody.transform;
            
            if (agent.path.corners.Length < 2) return;

            var nextCorner = agent.path.corners[1];
            var directionToTarget = nextCorner - transform.position;
            directionToTarget.y = 0;
            
            if (startDistance == 0 || Mathf.Abs(startDistance - directionToTarget.sqrMagnitude) > navigation.StopDistance)
            {
                startDistance = directionToTarget.sqrMagnitude;
            }
            
            isReversing = IsReversing(directionToTarget, out var steerMultiplier);
            CalculateSteerCorrection(steerMultiplier);

            var localTarget = transform.InverseTransformPoint(nextCorner);
            var steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);

            steer = isReversing ? steer * steerMultiplier : steer;
            steer += steerCorrection;
            steer = Mathf.Clamp(
                steer,
                -_vehicleMovementBaseStats.MaxSteerAngle,
                _vehicleMovementBaseStats.MaxSteerAngle
            );
            

            for (var i = 0; i < wheels.Count; i++)
            {
                var wheel = wheels[i];

                ProcessRearWheel(directionToTarget, wheel);

                if (wheel.axel == Axel.Front)
                {
                    ProcessFrontWheel(steer, wheel);
                }
            }

            agent.nextPosition = transform.position;
        }

        private bool IsReversing(Vector3 directionToTarget, out int steerMultiplier)
        {
            var signedAngleToTarget = Vector3.SignedAngle(rigidbody.transform.forward, directionToTarget, Vector3.up);
            
            CheckObstacles();

            // var result =
            //     (Mathf.Abs(signedAngleToTarget) > _vehicleMovementBaseStats.ReverseTargetAngle) ||
            //     hasObstacle[DirectionType.Forward] &&
            //     !hasObstacle[DirectionType.Back];
            
            var result = hasObstacle[DirectionType.Forward] && !hasObstacle[DirectionType.Back];
            
            steerMultiplier = signedAngleToTarget < 0 && result ? -1 : 1;

            Debug.DrawRay(rigidbody.transform.position, rigidbody.transform.forward * 20, Color.magenta);
            Debug.DrawRay(rigidbody.transform.position, directionToTarget, Color.cyan);

            return result;
        }

        private void CheckObstacles()
        {
            hasObstacle[DirectionType.Forward] = CheckObstacle(DirectionType.Forward);
            hasObstacle[DirectionType.ForwardRight] = CheckObstacle(DirectionType.ForwardRight);
            hasObstacle[DirectionType.ForwardLeft] = CheckObstacle(DirectionType.ForwardLeft);
            hasObstacle[DirectionType.Back] = CheckObstacle(DirectionType.Back);
            hasObstacle[DirectionType.BackRight] = CheckObstacle(DirectionType.BackRight);
            hasObstacle[DirectionType.BackLeft] = CheckObstacle(DirectionType.BackLeft);
            hasObstacle[DirectionType.Right] = CheckObstacle(DirectionType.Right);
            hasObstacle[DirectionType.Left] = CheckObstacle(DirectionType.Left);
        }

        private void CalculateSteerCorrection(int steerMultiplier)
        {
            if (hasObstacle.All(obstacle => !obstacle.Value))
            {
                steerCorrection = 0;
            }
            else
            {
                if (hasObstacle[DirectionType.Left])
                    steerCorrection += _vehicleMovementBaseStats.SteerAdjustment * steerMultiplier;

                if (hasObstacle[DirectionType.Right])
                    steerCorrection -= _vehicleMovementBaseStats.SteerAdjustment * steerMultiplier;
                
                if (hasObstacle[DirectionType.ForwardLeft])
                    steerCorrection += _vehicleMovementBaseStats.SteerAdjustment * steerMultiplier;

                if (hasObstacle[DirectionType.ForwardRight])
                    steerCorrection -= _vehicleMovementBaseStats.SteerAdjustment * steerMultiplier;
                
                if (hasObstacle[DirectionType.BackLeft] && isReversing)
                    steerCorrection -= _vehicleMovementBaseStats.SteerAdjustment * steerMultiplier;

                if (hasObstacle[DirectionType.BackRight] && isReversing)
                    steerCorrection += _vehicleMovementBaseStats.SteerAdjustment * steerMultiplier;
            
                if (hasObstacle[DirectionType.Back])
                    steerCorrection *= 0.5f;
            }
            
            steerCorrection = Mathf.Clamp(
                steerCorrection,
                -_vehicleMovementBaseStats.SteerAdjustment,
                _vehicleMovementBaseStats.SteerAdjustment
            );
        }

        private bool CheckObstacle(DirectionType direction)
        {
            var distance = _vehicleMovementBaseStats.ObstacleDistance(direction);
            var origins = obstacleTriggerOrigin[direction];

            for (int i = 0; i < origins.Length; i++)
            {
                var size = Physics.OverlapSphereNonAlloc(origins[i].position, distance, obstacles, ~_vehicleMovementBaseStats.ExcludeObstacleLayerMask);
                if (size > 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        private void ProcessFrontWheel(float steer, Wheel wheel)
        {
            var steerAngle = steer * _vehicleMovementBaseStats.MaxSteerAngle;

            steerAngle = Mathf.Clamp(
                steerAngle,
                -_vehicleMovementBaseStats.MaxSteerAngle,
                _vehicleMovementBaseStats.MaxSteerAngle);

            wheel.collider.steerAngle = Mathf.Lerp(
                wheel.collider.steerAngle, steerAngle, Time.deltaTime * _vehicleMovementBaseStats.RotationSpeed);
        }

        private void ProcessRearWheel(Vector3 directionToTarget, Wheel wheel)
        {
            var moveSpeed = isReversing ? -_vehicleMovementBaseStats.MoveSpeed : _vehicleMovementBaseStats.MoveSpeed;
            
            var distanceToMainTarget = (agent.transform.position - navigation.Target).sqrMagnitude;
            
            if (distanceToMainTarget < navigation.StopDistance * 2f && !isReversing)
            {
                wheel.collider.motorTorque = moveSpeed / 10;
            }
            else
            {
                wheel.collider.motorTorque = moveSpeed;
            }
        }
    }
}