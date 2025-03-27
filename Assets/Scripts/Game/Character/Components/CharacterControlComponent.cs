using Cinemachine;
using Configs.PlayerConfig;
using Game.CameraSystem;
using Game.CameraSystem.Components;
using Game.CameraSystem.Installers;
using Game.Car.Installers;
using Game.Character.Installers;
using Services.PlayerControlService;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Game.Character.Components
{
    public class CharacterControlComponentInstaller : Installer<CharacterControlComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CharacterControlComponent>().AsSingle().NonLazy();
        }
    }

    public class CharacterControlComponent
    {
        [Inject] private IPlayerControlService playerControlService;
        [Inject] private ICameraService cameraService;

        [Inject] private PlayerMovementBaseStats movementStats;

        [Inject] private CameraZoomComponent cameraZoomComponent;

        [Inject(Id = CharacterSystemInstaller.CHARACTRER_CONTROLLER)]
        private CharacterController characterController;

        [Inject(Id = CharacterSystemInstaller.CAMERA_FOLLOW_TARGET)]
        private Transform cameraFollowTarget;

        [Inject(Id = CharacterSystemInstaller.CHARACTER_GUN_PLACEMENT)]
        private Transform gunPlacement;

        [Inject(Id = CharacterSystemInstaller.HEAD_AIM)]
        private Transform headAim;

        [Inject(Id = CharacterSystemInstaller.CHARACTRER_VIEW)]
        private Transform view;

        [Inject(Id = CameraSystemInstaller.CAMERA_RAYCAST_POINTER)]
        private Transform cameraRaycastPointer;

        [Inject(Id = CameraSystemInstaller.GAMEPLAY_CAMERA)]
        private Camera camera;

        private CinemachineExternalCamera externalCamera;

        private float cameraRotation;
        private float bodyRotation;

        private Quaternion gunRotation;

        private Collider[] ground = new Collider[1];
        private Vector3 moveVector;
        private float verticalVelocity = 0;
        private float jumpTimeoutDelta;
        private float fallTimeoutDelta;

        public Vector3 Gravity => new Vector3(0.0f, verticalVelocity, 0.0f);

        private PlayerActionsProvider PlayerActionsProvider => playerControlService.PlayerActionsProvider;

        public Transform Transform => characterController.transform;

        public void Initialize()
        {
        }

        public void Control()
        {
            if (PlayerActionsProvider == null) return;

            ProcessMovement();
            ProcessJump();
            ProcessGravity();
            ProcessRotation();

            Move();
        }

        private void OnAimStart()
        {
            cameraService.RequestCameraTypeChange(GameCameraType.CharacterCombat);
        }

        private void OnAimEnd()
        {
            cameraService.RequestCameraTypeChange(GameCameraType.Character);
        }

        private void ProcessMovement()
        {
            var speed = PlayerActionsProvider.Sprint
                ? movementStats.SprintSpeed
                : movementStats.MoveSpeed;

            var forward = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized;
            var right = Vector3.ProjectOnPlane(camera.transform.right, Vector3.up).normalized;

            var moveDirection = new Vector3(PlayerActionsProvider.MoveVector.x, 0, PlayerActionsProvider.MoveVector.y);
            moveVector = forward * moveDirection.z + right * moveDirection.x;
            moveVector *= speed;
        }

        private void ProcessGravity()
        {
            if (IsGrounded())
            {
                fallTimeoutDelta = movementStats.FallTimeout;
            }
            else if (fallTimeoutDelta <= 0)
            {
                verticalVelocity += movementStats.Gravity * Time.deltaTime;
            }

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
        }

        private void ProcessJump()
        {
            if (IsGrounded())
            {
                if (PlayerActionsProvider.Jump && jumpTimeoutDelta <= 0.0f)
                {
                    verticalVelocity = movementStats.JumpForce;
                }

                if (jumpTimeoutDelta >= 0.0f)
                {
                    jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                jumpTimeoutDelta = movementStats.JumpTimeout;
            }
        }

        private void ProcessRotation()
        {
            cameraRotation += PlayerActionsProvider.LookVector.y * movementStats.LookMultiplier;
            bodyRotation = PlayerActionsProvider.LookVector.x * movementStats.LookMultiplier;

            cameraRotation = Mathf.Clamp(
                cameraRotation,
                movementStats.CameraVerticalClamp(cameraZoomComponent.ZoomFactor).x,
                movementStats.CameraVerticalClamp(cameraZoomComponent.ZoomFactor).y);

            cameraFollowTarget.localRotation = Quaternion.Euler(-cameraRotation, 0f, 0f);

            headAim.position = Vector3.Lerp(
                headAim.position,
                cameraRaycastPointer.position,
                Time.deltaTime * movementStats.RigLookSpeed
            );

            characterController.transform.Rotate(Vector3.up * bodyRotation);
        }

        private void Move()
        {
            moveVector = CollideAndSlide(
                moveVector,
                characterController.transform.position,
                0,
                false
                , moveVector
            );

            moveVector += CollideAndSlide(
                Gravity,
                characterController.transform.position + moveVector,
                0,
                true,
                Gravity);

            characterController.Move(
                moveVector * Time.deltaTime +
                new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        }


        private bool IsGrounded()
        {
            return characterController.isGrounded;
        }

        public void SetActive(bool isActive)
        {
            characterController.enabled = isActive;


            if (isActive)
            {
                PlayerActionsProvider.OnAimStartAction += OnAimStart;
                PlayerActionsProvider.OnAimEndAction += OnAimEnd;
            }
            else
            {
                PlayerActionsProvider.OnAimStartAction -= OnAimStart;
                PlayerActionsProvider.OnAimEndAction -= OnAimEnd;
            }
        }

        private Vector3 CollideAndSlide(
            Vector3 velocity,
            Vector3 position,
            int depth,
            bool gravityPass,
            Vector3 velocityInit
        )
        {
            if (depth > movementStats.MaxBounces)
            {
                return Vector3.zero;
            }

            var distance = velocity.magnitude + characterController.skinWidth;

            if (CapsuleCastFromCharacter(velocity.normalized, distance, out var hit, movementStats.SlideLayerMask))
            {
                var snapToSurface = velocity.normalized * (hit.distance - characterController.skinWidth);
                var leftOver = velocity - snapToSurface;
                var angle = Vector3.Angle(Vector3.up, hit.normal);

                if (snapToSurface.magnitude <= characterController.skinWidth)
                {
                    snapToSurface = Vector3.zero;
                }

                if (angle <= movementStats.MaxSlopeAngle)
                {
                    if (gravityPass)
                    {
                        return snapToSurface;
                    }

                    leftOver = ProjectAndScale(leftOver, hit.normal);
                }
                else
                {
                    var scale = 1 - Vector3.Dot(
                        new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
                        -new Vector3(velocityInit.x, 0, velocityInit.z).normalized
                    );

                    if (IsGrounded() && !gravityPass)
                    {
                        leftOver = ProjectAndScale(
                            new Vector3(leftOver.x, 0, leftOver.z),
                            new Vector3(hit.normal.x, 0, hit.normal.z)
                        ).normalized;

                        leftOver *= scale;
                    }
                    else
                    {
                        leftOver = ProjectAndScale(leftOver, hit.normal) * scale;
                    }
                }


                return snapToSurface +
                       CollideAndSlide(
                           leftOver, position + snapToSurface, depth + 1, gravityPass, velocityInit
                       );
            }

            return velocity;
        }

        private static Vector3 ProjectAndScale(Vector3 leftOver, Vector3 hitNormal)
        {
            var leftOverMagnitude = leftOver.magnitude;
            leftOver = Vector3.ProjectOnPlane(leftOver, hitNormal).normalized;
            leftOver *= leftOverMagnitude;
            return leftOver;
        }

        private bool CapsuleCastFromCharacter(Vector3 direction, float maxDistance, out RaycastHit hit,
            LayerMask layerMask)
        {
            float height = characterController.height;
            float radius = characterController.radius;
            Vector3 center = characterController.transform.position + characterController.center;

            Vector3 bottom = center + Vector3.down * (height / 2 - radius);
            Vector3 top = center + Vector3.up * (height / 2 - radius);

            return Physics.CapsuleCast(bottom, top, radius, direction, out hit, maxDistance, layerMask);
        }
    }
}