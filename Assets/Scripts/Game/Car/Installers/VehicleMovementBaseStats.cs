using UnityEngine;

namespace Game.Car.Installers
{
    [CreateAssetMenu(menuName = "Configs/Player/Movement/VehicleMovement", fileName = "VehicleMovementBaseStats")]
    public class VehicleMovementBaseStats : ScriptableObject
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float maxSteerAngle;
        [SerializeField] private float stopBrakeTorque;
        [SerializeField] private float brakeTorque;
        
        public float MoveSpeed => moveSpeed;
        public float RotationSpeed => rotationSpeed;
        public float MaxSteerAngle => maxSteerAngle;
        public float StopBrakeTorque => stopBrakeTorque;
        public float BrakeTorque => brakeTorque;
    }
}