using System.Collections.Generic;
using UI.TransmissionSettings.Components;
using UnityEngine;
using Zenject;

namespace UI.TransmissionSettings
{
    public class TransmissionSettingsScreenInstaller : MonoInstaller
    {
        [SerializeField] private TransmissionSettingsMenu settingsMenu;
        
        public override void InstallBindings()
        {
            Container.BindInstance(settingsMenu);
        }
    }
}