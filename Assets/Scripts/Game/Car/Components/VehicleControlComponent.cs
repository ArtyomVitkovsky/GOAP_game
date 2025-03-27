using System.Collections.Generic;
using Game.Car.Installers;
using UnityEngine;
using Zenject;

namespace Game.Car.Components
{
    public abstract class VehicleControlComponent
    {
        [Inject(Id = VehicleSystemInstaller.VEHICLE_RIGIDBODY)]
        protected Rigidbody rigidbody;
        [Inject(Id = VehicleSystemInstaller.VEHICLE_WHEELS)]
        protected List<Wheel> wheels;

        [Inject] protected VehicleMovementBaseStats _vehicleMovementBaseStats;

        protected abstract bool IsInitialized { get; set; }

        public Vector2 MoveVector;
        public bool HandBreak;
        
        public abstract void Initialize();
        public abstract void Dispose();
        
        public bool IsAllWheelsGrounded()
        {
            var wheelsGroundedCount = WheelsGroundedCount();

            return wheelsGroundedCount == wheels.Count;
        }
        
        public bool IsHalfOfWheelsGrounded()
        {
            var wheelsGroundedCount = WheelsGroundedCount();

            return wheelsGroundedCount >= wheels.Count / 2;
        }
        
        public bool IsTwoWheelsGrounded()
        {
            var wheelsGroundedCount = WheelsGroundedCount();

            return wheelsGroundedCount >= 2;
        }

        private int WheelsGroundedCount()
        {
            var wheelsGroundedCount = 0;

            for (var i = 0; i < wheels.Count; i++)
            {
                wheels[i].collider.GetGroundHit(out var hit);
                if (hit.collider != null)
                {
                    wheelsGroundedCount++;
                }
            }

            return wheelsGroundedCount;
        }

        public virtual bool Stop()
        {
            var isStoppped = true;
            
            for (int i = 0; i < wheels.Count; i++)
            {
                wheels[i].collider.motorTorque = 0;
                wheels[i].collider.brakeTorque = _vehicleMovementBaseStats.StopBrakeTorque;

                if (wheels[i].collider.rpm != 0)
                {
                    isStoppped = false;
                }
            }

            return isStoppped;
        }

        public void ReleaseBreaks()
        {
            for (int i = 0; i < wheels.Count; i++)
            {
                wheels[i].collider.motorTorque = 0;
                wheels[i].collider.brakeTorque = 0;
            }
        }

        public virtual void Control()
        {
            if (!IsInitialized)
            {
                return;
            }
            
            WheelsControl(MoveVector);
        }

        protected void WheelsControl(Vector2 moveVector)
        {
            for (var i = 0; i < wheels.Count; i++)
            {
                var wheel = wheels[i];
                if (wheel.axel == Axel.Rear)
                {
                    var motorTorque = moveVector.y * _vehicleMovementBaseStats.MoveSpeed;
                    wheel.collider.motorTorque = motorTorque;
                    wheel.collider.brakeTorque = HandBreak ? 5000 : 0;
                }
                else if (wheel.axel == Axel.Front)
                {
                    var steerAngle =
                        moveVector.x *
                        _vehicleMovementBaseStats.RotationSpeed *
                        _vehicleMovementBaseStats.MaxSteerAngle;

                    steerAngle = Mathf.Clamp(
                        steerAngle,
                        -_vehicleMovementBaseStats.MaxSteerAngle,
                        _vehicleMovementBaseStats.MaxSteerAngle);

                    wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, steerAngle, 0.6f);
                }
            }
        }

    }
}