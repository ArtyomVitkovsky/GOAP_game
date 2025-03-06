using UnityEngine;

namespace Configs.PlayerConfig
{
    [CreateAssetMenu(menuName = "Configs/Player/Movement/PlayerMovement", fileName = "PlayerMovementBaseStats")]
    public class PlayerMovementBaseStats : ScriptableObject
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float jumpMultiplier;
        [SerializeField] private float maxVelocity;
        [SerializeField] private float maxSprintVelocity;
        [SerializeField] private float groundedCheckerDistance;
        [SerializeField] private float groundDrag;
    
        public float MoveSpeed => moveSpeed;
        public float SprintSpeed => sprintSpeed;
        public float JumpMultiplier => jumpMultiplier;
        public float MaxVelocity => maxVelocity;
        public float MaxSprintVelocity => maxSprintVelocity;
        public float GroundedCheckerDistance => groundedCheckerDistance;
        public float GroundDrag => groundDrag;
    }
}