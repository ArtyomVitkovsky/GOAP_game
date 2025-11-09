using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.PlayerConfig
{
    [CreateAssetMenu(menuName = "Configs/Player/Movement/PlayerMovement", fileName = "PlayerMovementBaseStats")]
    public class PlayerMovementBaseStats : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeed;

        [Header("Collide And Slide")] 
        [SerializeField] private int maxBounces;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private LayerMask slideLayerMask;
        
        [Header("Jump and Gravity")]
        [SerializeField] private float gravity;
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpTimeout;
        [SerializeField] private float fallTimeout;

        [Header("Look")] 
        [SerializeField] private float lookMultiplier;
        [SerializeField] private Vector2 lookSensitivity;
        [SerializeField] private Vector2 lookSmoothing;
        [SerializeField] private Vector2 clampInDegrees = new Vector2(360, 180);
        [SerializeField] private float rigLookSpeed;
        [SerializeField] private Vector2 cameraVerticalClampMin;
        [SerializeField] private Vector2 cameraVerticalClampMax;
        
        [Header("Zoom")]
        [SerializeField] private float maxOffset;
        [SerializeField] private float minOffset;
        [SerializeField] private float zoomSpeed;
        
        
        public float LookMultiplier => lookMultiplier;
        public Vector2 LookSensitivity => lookSensitivity;
        public Vector2 LookSmoothing => lookSmoothing;
        public Vector2 ClampInDegrees => clampInDegrees;
        public float RigLookSpeed => rigLookSpeed;

        public int MaxBounces => maxBounces;
        public float MaxSlopeAngle => maxSlopeAngle;
        public LayerMask SlideLayerMask => slideLayerMask;

        
        public float MoveSpeed => moveSpeed;
        public float SprintSpeed => sprintSpeed;
        
        
        public float Gravity => gravity;
        public float JumpForce => jumpForce;
        public float JumpTimeout => jumpTimeout;
        public float FallTimeout => fallTimeout;
        
        
        public float MaxOffset => maxOffset;
        public float MinOffset => minOffset;
        public float ZoomSpeed => zoomSpeed;
        
        
        public Vector2 CameraVerticalClamp(float zoomFactor)
        {
            return new Vector2(
                Mathf.Lerp(cameraVerticalClampMin.x, cameraVerticalClampMax.x, zoomFactor),
                Mathf.Lerp(cameraVerticalClampMin.y, cameraVerticalClampMax.y, zoomFactor)
            );
        }
    }
}