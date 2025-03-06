using Game.NpcSystem.Components;
using Zenject;

namespace Game.NpcSystem.Installers
{
    public class NpcEncounterComponentInstaller : Installer<NpcEncounterComponentInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NpcEncounterComponent>().AsSingle().NonLazy();
        }
    }
}