using System;
using UnityEngine;

namespace Game.Turret
{
    public class TurretView : MonoBehaviour
    {
        [Header("Transforms")]
        [SerializeField] private Transform body;
        [SerializeField] private Transform barrelAnchor;
        [SerializeField] private Transform projectileAnchor;
        
        [Space]
        [Header("Transforms")]
        [SerializeField] private float bodyRotationSpeed;
        [SerializeField] private float barrelRotationSpeed;

        [SerializeField] private LayerMask layerMask;
        private RaycastHit pointerHit;
        
        private Transform turretPointer;

        public Transform ProjectileAnchor => projectileAnchor;

        public void SetPointer(Transform pointer)
        {
            turretPointer = pointer;
        }

        public void LookAtTarget(Transform target)
        {
            RotateBodyToTarget(target);
            RotateBarrelToTarget(target);
            SetPointerPosition();
        }

        public void ResetRotation()
        {
            var lookRotation = Quaternion.Euler(Vector3.zero);

            body.rotation = lookRotation;

            barrelAnchor.localRotation = lookRotation;
        }

        private void RotateBodyToTarget(Transform target)
        {
            var targetPosition = new Vector3(target.position.x, body.position.y, target.position.z);
            LookAtTarget(body, targetPosition, bodyRotationSpeed);
        }

        private void RotateBarrelToTarget(Transform target)
        {
            Vector3 barrelDirection = target.position - barrelAnchor.position;
            Quaternion newRotation = Quaternion.LookRotation(barrelDirection, Vector3.up);

            newRotation.y = 0.0f;
            newRotation.z = 0.0f;

            Vector3 euler = newRotation.eulerAngles;
            if (euler.x > 180) euler.x -= 360;
            euler.x = Mathf.Clamp(euler.x, -60, 10);

            Quaternion barrelTargetRotation = Quaternion.Euler(euler.x, barrelAnchor.localRotation.eulerAngles.y, barrelAnchor.localRotation.eulerAngles.z);
            barrelAnchor.localRotation = Quaternion.Slerp(barrelAnchor.localRotation, barrelTargetRotation, Time.deltaTime * barrelRotationSpeed);
        }
        
        private void LookAtTarget(Transform rotatedBody, Vector3 targetPosition, float rotationSpeed)
        {
            var lookRotation = Quaternion.LookRotation(targetPosition - body.position);
            rotatedBody.rotation =
                Quaternion.RotateTowards(rotatedBody.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        private void SetPointerPosition()
        {
            var ray = new Ray(projectileAnchor.position, projectileAnchor.forward);
                
            if (Physics.Raycast(ray, out pointerHit, float.PositiveInfinity, ~layerMask))
            {
                turretPointer.position = pointerHit.point;
            }
        }
    }
}