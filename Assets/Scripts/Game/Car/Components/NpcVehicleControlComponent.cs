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
        private Transform obstacleTriggerOrigin;

        private float startDistance;
        private bool isReversing;
        private NpcNavigationComponent navigation;
        private NavMeshAgent agent;

        private Vector3 startPosition;

        public override void Initialize()
        {
            ReleaseBreaks();
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
            agent.updateRotation = false;
            agent.updatePosition = false;

            
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
            
            isReversing = IsReversing(directionToTarget);

            var localTarget = transform.InverseTransformPoint(nextCorner);
            var steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);

            steer = isReversing ? -steer : steer;

            for (var i = 0; i < wheels.Count; i++)
            {
                var wheel = wheels[i];

                if (wheel.axel == Axel.Rear)
                {
                    ProcessRearWheel(directionToTarget, wheel);
                }
                else if (wheel.axel == Axel.Front)
                {
                    ProcessFrontWheel(steer, wheel);
                }
            }

            agent.nextPosition = transform.position;
        }

        private bool IsReversing(Vector3 directionToTarget)
        {
            var dot = Vector3.Dot(rigidbody.transform.forward, directionToTarget);

            var direction = obstacleTriggerOrigin.transform.forward;
            var ray = new Ray(obstacleTriggerOrigin.position, direction * 6);
            Debug.DrawRay(obstacleTriggerOrigin.position, direction, Color.red);
            var isObstacleAhead = Physics.Raycast(ray, 6);

            return dot < 0 || isObstacleAhead;
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
            
            if (directionToTarget.sqrMagnitude < navigation.StopDistance * 1.5f)
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