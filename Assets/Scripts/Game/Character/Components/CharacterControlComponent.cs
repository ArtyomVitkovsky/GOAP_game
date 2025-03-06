using Configs.PlayerConfig;
using Game.CameraSystem;
using Game.CameraSystem.Installers;
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
        [Inject] private ITickableService tickableService;
        [Inject] private ICameraService cameraService;

        [Inject] private PlayerMovementBaseStats playerMovementBaseStats;

        [Inject(Id = CharacterSystemInstaller.CHARACTRER_RIGIDBODY)]
        private Rigidbody rigidbody;

        [Inject(Id = CharacterSystemInstaller.CHARACTRER_COLLIDER)]
        private CapsuleCollider capsuleCollider;

        [Inject(Id = CharacterSystemInstaller.CHARACTRER_VIEW)]
        private Transform view;

        [Inject(Id = CameraSystemInstaller.CAMERA_RAYCAST_POINTER)]
        private Transform cameraRaycastPointer;

        [Inject(Id = CameraSystemInstaller.GAMEPLAY_CAMERA)]
        private Camera camera;

        private Quaternion targetLookRotation;
        private Quaternion targetViewRotation;

        private RaycastHit groundRaycastHit;
        private PlayerActionsProvider PlayerActionsProvider => playerControlService.PlayerActionsProvider;

        public Transform Transform => rigidbody.transform;

        public void Initialize()
        {
        }

        public void Control()
        {
            if (PlayerActionsProvider == null) return;

            ProcessMovement();
            ProcessJump();
            ProcessGroundDrag();
            ProcessRotation();
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
            if (PlayerActionsProvider.Jump || !IsGrounded()) return;
            if (PlayerActionsProvider.MoveVector.x == 0 && PlayerActionsProvider.MoveVector.y == 0) return;

            var speed = PlayerActionsProvider.Sprint
                ? playerMovementBaseStats.SprintSpeed
                : playerMovementBaseStats.MoveSpeed;

            var maxVelocity = PlayerActionsProvider.Sprint
                ? playerMovementBaseStats.MaxSprintVelocity
                : playerMovementBaseStats.MaxVelocity;

            var moveDirection = new Vector3(PlayerActionsProvider.MoveVector.x, 0, PlayerActionsProvider.MoveVector.y);
            var moveVector = rigidbody.transform.TransformDirection(moveDirection);

            rigidbody.AddForce(moveVector * speed, ForceMode.Force);
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);
        }

        private void ProcessGroundDrag()
        {
            rigidbody.drag = IsGrounded() ? playerMovementBaseStats.GroundDrag : 0;
        }

        private void ProcessJump()
        {
            if (!PlayerActionsProvider.Jump || !IsGrounded()) return;

            rigidbody.AddForce(Vector3.up * playerMovementBaseStats.JumpMultiplier, ForceMode.Impulse);
        }

        private void ProcessRotation()
        {
            RotateBody();

            RotateView();
        }

        private void RotateBody()
        {
            if (PlayerActionsProvider.MoveVector.x != 0 ||
                PlayerActionsProvider.MoveVector.y != 0 && !PlayerActionsProvider.Aim)
            {
                var targetPosition = new Vector3(
                    cameraRaycastPointer.position.x,
                    rigidbody.transform.position.y,
                    cameraRaycastPointer.position.z
                );

                targetLookRotation = Quaternion.LookRotation(targetPosition - rigidbody.transform.position);
            }

            if (rigidbody.rotation != targetLookRotation && !PlayerActionsProvider.Aim)
            {
                rigidbody.MoveRotation(
                    Quaternion.Slerp(rigidbody.rotation, targetLookRotation.normalized, Time.deltaTime * 10)
                );
            }

            if (PlayerActionsProvider.Aim)
            {
                var targetPosition = new Vector3(
                    cameraRaycastPointer.position.x,
                    rigidbody.transform.position.y,
                    cameraRaycastPointer.position.z
                );
                var aimDirection = (targetPosition - rigidbody.transform.position).normalized;
                rigidbody.MoveRotation(Quaternion.LookRotation(aimDirection));
            }
        }

        private void RotateView()
        {
            if (PlayerActionsProvider.Aim)
            {
                view.transform.localRotation = Quaternion.identity;
                return;
            }

            if (PlayerActionsProvider.MoveVector.x != 0 || PlayerActionsProvider.MoveVector.y != 0)
            {
                var sideDirection = rigidbody.transform.right * PlayerActionsProvider.MoveVector.x;
                var frontDirection = rigidbody.transform.forward * PlayerActionsProvider.MoveVector.y;
                targetViewRotation = Quaternion.LookRotation((sideDirection + frontDirection).normalized);
            }

            if (view.transform.rotation != targetViewRotation)
            {
                view.transform.rotation =
                    Quaternion.Slerp(view.transform.rotation, targetViewRotation, Time.deltaTime * 10);
            }
        }

        private bool IsGrounded()
        {
            var origin = rigidbody.transform.position;
            var radius = capsuleCollider.radius;
            var direction = -rigidbody.transform.up;

            return Physics.SphereCast(
                origin, radius, direction, out groundRaycastHit,
                playerMovementBaseStats.GroundedCheckerDistance
            );
        }

        public void SetActive(bool isActive)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = !isActive;
            capsuleCollider.enabled = isActive;

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
    }
}