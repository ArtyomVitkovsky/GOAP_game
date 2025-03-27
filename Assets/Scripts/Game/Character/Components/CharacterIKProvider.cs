using System;
using Game.Character.Installers;
using Services.PlayerControlService;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

namespace Game.Character.Components
{
    public class CharacterIKProvider : MonoBehaviour
    {
        [Inject] private IPlayerControlService playerControlService;
        
        [Inject(Id = CharacterSystemInstaller.CHARACTRER_ANIMATOR)]
        private Animator animator;
        
        [Inject(Id = CharacterSystemInstaller.CHARACTRER_CONTROLLER)]
        private CharacterController characterController;
        
        [Range(0, 1f)] [SerializeField] private float DistanceToGround;
        [Range(0, 1f)] [SerializeField] private float LegLentgh;
        [Range(0, 0.1f)] [SerializeField] private float sphereRadius;

        [SerializeField] private LayerMask layerMask;
        
        private Ray rightFootRay;
        private Vector3 rightFootPosition;
        private RaycastHit rightFootHit;
        
        private Ray leftFootRay;
        private Vector3 leftFootPosition;
        private RaycastHit leftFootHit;
        
        private float leftWeight;
        private float rightWeight;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawSphere(rightFootHit.point, sphereRadius);
            Gizmos.DrawSphere(leftFootHit.point, sphereRadius);
            
            Debug.DrawRay(rightFootPosition, rightFootHit.normal, Color.cyan);
            Debug.DrawRay(leftFootPosition, leftFootHit.normal, Color.cyan);
            
            Debug.DrawLine(rightFootPosition, rightFootHit.point, Color.yellow);
            Debug.DrawLine(leftFootPosition, leftFootHit.point, Color.yellow);
            
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (characterController.isGrounded && 
                playerControlService.PlayerActionsProvider.MoveVector.sqrMagnitude <= 0)
            {
                leftWeight = animator.GetFloat(CharacterAnimationParameters.LeftFootIKWeight);
                rightWeight = animator.GetFloat(CharacterAnimationParameters.RightFootIKWeight);

                SetupFootIk(AvatarIKGoal.LeftFoot, HumanBodyBones.LeftLowerLeg,
                    leftWeight,
                    out leftFootRay, out leftFootHit, out leftFootPosition);
            
                SetupFootIk(AvatarIKGoal.RightFoot, HumanBodyBones.RightLowerLeg,
                    rightWeight,
                    out rightFootRay, out rightFootHit, out rightFootPosition);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
                
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            }
        }
        
        private void LateUpdate()
        {
            if (!characterController.isGrounded || 
                playerControlService.PlayerActionsProvider.MoveVector.sqrMagnitude > 0)
            {
                animator.transform.localPosition = Vector3.zero;
                return;
            }
            
            var leftFootDiff = leftFootHit.point.y - leftFootPosition.y;
            var rightFootDiff = leftFootHit.point.y - leftFootPosition.y;
            float averageDiff = (Mathf.Abs(leftFootDiff) + Mathf.Abs(rightFootDiff)) / 2;
            float averageHeight = -Mathf.Abs(leftFootHit.point.y - rightFootHit.point.y) / 2;
            Vector3 targetPosition = new Vector3(0, averageHeight - averageDiff, 0);
        
            float smoothingFactor = 0.1f;
            animator.transform.localPosition = Vector3.Lerp(animator.transform.localPosition, targetPosition, smoothingFactor);
        }

        private void SetupFootIk(AvatarIKGoal goal, HumanBodyBones bone, float weight,
            out Ray ray, out RaycastHit hit, out Vector3 footPosition)
        {
            animator.SetIKPositionWeight(goal, 1);
            animator.SetIKRotationWeight(goal, 1);
            
            footPosition = animator.GetIKPosition(goal);

            var boneTransform = animator.GetBoneTransform(bone);
            ray = new Ray(boneTransform.position, -Vector3.up);
            Debug.DrawRay(ray.origin, ray.direction * (DistanceToGround + 1f), Color.green);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, ~layerMask))
            {
                footPosition = hit.point;
                footPosition.y += DistanceToGround;
                animator.SetIKPosition(goal, footPosition);
                Quaternion footRotation = 
                    Quaternion.LookRotation(
                    Vector3.ProjectOnPlane(transform.forward, hit.normal),
                    hit.normal);
                animator.SetIKRotation(goal, footRotation);
            }
        }
    }
}