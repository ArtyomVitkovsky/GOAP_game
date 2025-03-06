using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainScreen.Installers
{
    public class MainScreenInstaller : MonoInstaller
    {
        public const string CROSSHAIR = "CROSSHAIR";
        public const string TURRETS_CROSSHAIR = "TURRETS_CROSSHAIR";
        public const string INTERACT_IMAGE = "INTERACT_IMAGE";
    
        [SerializeField] private Image crosshair;
        [SerializeField] private Image turretsCrosshair;
        [SerializeField] private Image interactImage;

        public override void InstallBindings()
        {
            Container.BindInstance(crosshair).WithId(CROSSHAIR);
            Container.BindInstance(turretsCrosshair).WithId(TURRETS_CROSSHAIR);
            Container.BindInstance(interactImage).WithId(INTERACT_IMAGE);
        
            CrosshairComponentInstaller.Install(Container);
            InteractionUserInterfaceComponentInstaller.Install(Container);
        }
    }
}