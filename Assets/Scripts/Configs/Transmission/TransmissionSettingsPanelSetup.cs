using UI.TransmissionSettings;
using UnityEngine;

namespace Configs.Transmission
{
    public abstract class TransmissionSettingsPanelSetup : ScriptableObject
    {
        public abstract TransmissionSettingsType SettingsType { get; }
    }
}