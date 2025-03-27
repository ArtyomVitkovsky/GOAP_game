using System;
using Cysharp.Threading.Tasks;
using Game.Character.Installers;
using Services.PlayerControlService;
using Services.TickableService;
using UnityEngine;
using Zenject;

namespace Game.Character.Components
{
    public class CharacterAnimationComponentInstaller : Installer<CharacterAnimationComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CharacterAnimationComponent>().AsSingle().NonLazy();
        }
    }

    public static class CharacterAnimationParameters
    {
        public static int Idle = Animator.StringToHash("Idle");
        public static int Walk = Animator.StringToHash("Walk");
        public static int Run = Animator.StringToHash("Run");
        
        public static int IdleToSprint = Animator.StringToHash("IdleToSprint");
        public static int Sprint = Animator.StringToHash("Sprint");
        public static int RightDiagonalSprint = Animator.StringToHash("RightDiagonalSprint");
        public static int LeftDiagonalSprint = Animator.StringToHash("RightDiagonalSprint");
        public static int RightSprint = Animator.StringToHash("RightSprint");
        public static int LeftSprint = Animator.StringToHash("LeftSprint");
        
        public static int Jump = Animator.StringToHash("Jump");
        
        public static int CrouchIdle = Animator.StringToHash("CrouchIdle");
        public static int CrouchWalk = Animator.StringToHash("CrouchWalk");
        
        public static string LeftFootIKWeight = "LeftFootIKWeight";
        public static string RightFootIKWeight = "RightFootIKWeight";
    }

    public class CharacterAnimationStateMachine
    {
        public int GetState(PlayerActionsProvider playerActionsProvider)
        {
            if (playerActionsProvider.Sprint)
            {
                Debug.LogError($"Sprint");
                return CharacterAnimationParameters.Sprint;
            }
            else if (playerActionsProvider.MoveVector.sqrMagnitude > 0)
            {
                if (playerActionsProvider.Crouch)
                {
                    Debug.LogError($"CrouchWalk");
                    return CharacterAnimationParameters.CrouchWalk;
                }
                else
                {
                    Debug.LogError($"Run");
                    return CharacterAnimationParameters.Run;
                }
            }
            else if (playerActionsProvider.Crouch)
            {
                Debug.LogError($"CrouchIdle");
                return CharacterAnimationParameters.CrouchIdle;
            }
            
            return CharacterAnimationParameters.Idle;
        }
    }

    public class CharacterAnimationComponent
    {
        [Inject] private IPlayerControlService playerControlService;
        [Inject] private ITickableService tickableService;
        
        [Inject(Id = CharacterSystemInstaller.CHARACTRER_ANIMATOR)] 
        private Animator animator;

        private int currentTrigger;
        private TickableEntity _tickableEntity;

        private int newTrigger;

        private CharacterAnimationStateMachine animationStateMachine = new CharacterAnimationStateMachine();

        public void Initialize()
        {
            _tickableEntity = new TickableEntity(ProcessAnimations);
            tickableService.AddUpdateTickable(_tickableEntity);
        }

        private void ProcessAnimations()
        {
            newTrigger = animationStateMachine.GetState(playerControlService.PlayerActionsProvider);

            if (newTrigger == currentTrigger)
            {
                return;
            }
            
            animator.SetBool(currentTrigger, false);

            currentTrigger = newTrigger;
            animator.SetBool(currentTrigger, true);
        }
    }
}