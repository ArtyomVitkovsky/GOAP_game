using Game.Character.Components;
using UnityEngine;
using Zenject;

namespace Game.Character.Installers
{
    public class CharacterSystemInstaller : MonoInstaller
    {
        public const string CHARACTRER_CONTROLLER = "CHARACTRER_CONTROLLER";
        public const string CHARACTRER_ANIMATOR = "CHARACTRER_ANIMATOR";
        public const string CHARACTRER_IK_PROVIDER = "CHARACTRER_IK_PROVIDER";
        public const string CHARACTRER_VIEW = "CHARACTRER_VIEW";
        public const string CHARACTER_INTERACTION_SETUP = "CHARACTER_INTERACTION_SETUP";
        public const string CHARACTER_INTERACTION_ORIGIN_POINT = "CHARACTER_INTERACTION_ORIGIN_POINT";
        public const string CHARACTER_GUN_PLACEMENT = "CHARACTER_GUN_PLACEMENT";
        public const string CAMERA_FOLLOW_TARGET = "CAMERA_FOLLOW_TARGET";
        public const string HEAD_AIM = "HEAD_AIM";

        [Header("Instances")] 
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterIKProvider ikProvider;
        [SerializeField] private Transform view;
        [SerializeField] private Transform originPoint;
        [SerializeField] private CharacterInteractionSetup interactionSetup;
        [SerializeField] private Transform gunPlacement;
        [SerializeField] private Transform cameraFollowTarget;

        [Header("Rig")] [SerializeField] private Transform headAim;


        public override void InstallBindings()
        {
            BindInstances();

            BindComponents();
        }

        private void BindInstances()
        {
            Container.BindInstance(characterController).WithId(CHARACTRER_CONTROLLER);
            
            Container.BindInstance(animator).WithId(CHARACTRER_ANIMATOR);
            Container.BindInstance(ikProvider).WithId(CHARACTRER_IK_PROVIDER);
            
            Container.BindInstance(view).WithId(CHARACTRER_VIEW);
            Container.BindInstance(interactionSetup).WithId(CHARACTER_INTERACTION_SETUP);
            Container.BindInstance(originPoint).WithId(CHARACTER_INTERACTION_ORIGIN_POINT);
            Container.BindInstance(gunPlacement).WithId(CHARACTER_GUN_PLACEMENT);
            Container.BindInstance(cameraFollowTarget).WithId(CAMERA_FOLLOW_TARGET);
            Container.BindInstance(headAim).WithId(HEAD_AIM);
        }

        private void BindComponents()
        {
            CharacterControlComponentInstaller.Install(Container);
            // CharacterAnimationComponentInstaller.Install(Container);
            CharacterInteractionComponentInstaller.Install(Container);
            CharacterCombatComponentInstaller.Install(Container);
        }
    }
}