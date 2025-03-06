using Zenject;

namespace Services.NpcLifeService
{
    public class NpcLifeServiceInstaller : Installer<NpcLifeServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NpcLifeService>().AsSingle().NonLazy();
        }
    }
}