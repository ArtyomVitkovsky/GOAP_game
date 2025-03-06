using Zenject;

namespace Game.Character.Components
{
    public class CombatServiceInstaller : Installer<CombatServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CombatService>().AsSingle().NonLazy();
        }
    }
}