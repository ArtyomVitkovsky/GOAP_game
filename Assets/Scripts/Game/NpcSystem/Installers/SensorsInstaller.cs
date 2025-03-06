using System.Collections.Generic;
using AI.Sensors;
using UnityEngine;
using Zenject;

namespace Game.NpcSystem.Installers
{
    public class SensorsInstaller : MonoInstaller
    {
        [SerializeField] private List<BaseSensor<RaycastHit>> rayCastSensors;
        
        public override void InstallBindings()
        {
            Container.BindInstance(rayCastSensors);
        }
    }
}