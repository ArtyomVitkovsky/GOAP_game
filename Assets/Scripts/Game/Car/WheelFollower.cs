using Services.TickableService;
using UnityEngine;

namespace Game.Car
{
    public class WheelFollower : Follower
    {
        [SerializeField] private WheelCollider wheel;

        private Vector3 wheelPosition;
        private Quaternion wheelRotation;
        
        protected override void AddToTickable()
        {
            tickableService.AddFixedUpdateTickable(new TickableEntity(Follow));
        }

        protected override void Follow()
        {
            wheel.GetWorldPose(out wheelPosition, out wheelRotation);
            transform.position = wheelPosition;
            transform.rotation = wheelRotation;
        }
    }
}