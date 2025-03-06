using System;
using Cinemachine;
using Game.Character.Components;
using UnityEngine;
using Zenject;

namespace Game.Character.Installers
{
    public class CharacterSystemInstaller : MonoInstaller
    {
        public const string CHARACTRER_RIGIDBODY = "CHARACTRER_RIGIDBODY";
        public const string CHARACTRER_VIEW = "CHARACTRER_VIEW";
        public const string CHARACTRER_COLLIDER = "CHARACTRER_COLLIDER";
        public const string CHARACTER_INTERACTION_SETUP = "CHARACTER_INTERACTION_SETUP";
        public const string CHARACTER_INTERACTION_ORIGIN_POINT = "CHARACTER_INTERACTION_ORIGIN_POINT";
        public const string CHARACTER_GUN_PLACEMENT = "CHARACTER_GUN_PLACEMENT";
        
        [Header("Instances")]
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Transform view;
        [SerializeField] private Transform originPoint;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private CharacterInteractionSetup interactionSetup;
        [SerializeField] private Transform gunPlacement;
    
        public override void InstallBindings()
        {
            BindInstances();
        
            BindComponents();
        }
        
        private void BindInstances()
        {
            Container.BindInstance(rigidbody).WithId(CHARACTRER_RIGIDBODY);
            Container.BindInstance(view).WithId(CHARACTRER_VIEW);
            Container.BindInstance(capsuleCollider).WithId(CHARACTRER_COLLIDER);
            Container.BindInstance(interactionSetup).WithId(CHARACTER_INTERACTION_SETUP);
            Container.BindInstance(originPoint).WithId(CHARACTER_INTERACTION_ORIGIN_POINT);
            Container.BindInstance(gunPlacement).WithId(CHARACTER_GUN_PLACEMENT);
        }

        private void BindComponents()
        {
            CharacterControlComponentInstaller.Install(Container);
            CharacterInteractionComponentInstaller.Install(Container);
        }
    }
}