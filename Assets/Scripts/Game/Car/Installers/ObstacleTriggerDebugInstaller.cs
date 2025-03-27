using UnityEngine;
using Zenject;

namespace Game.Car.Installers
{
    public class ObstacleTriggerDebugInstaller : MonoInstaller
    {
        [SerializeField] private ObstacleTriggerDebug obstacleTriggerDebug;
        public override void InstallBindings()
        {
            Container.BindInstance(obstacleTriggerDebug);
        }
    }
}