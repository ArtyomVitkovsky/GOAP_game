using Services.PlayerControlService;
using Zenject;

namespace Game.Character.Components
{
    public class CharacterCombatComponentInstaller : Installer<CharacterCombatComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CharacterCombatComponent>().AsSingle().NonLazy();
        }
    }
    
    public class CharacterCombatComponent
    {
        [Inject] private IPlayerControlService playerControlService;

        [Inject] private CameraSystem.CameraSystem cameraSystem;

        public void Initialize()
        {
            
        }
    }
}