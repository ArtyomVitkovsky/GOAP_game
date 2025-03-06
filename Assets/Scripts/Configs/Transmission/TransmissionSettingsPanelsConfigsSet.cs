using System.Collections.Generic;
using System.Linq;
using UI.TransmissionSettings;
using UnityEngine;

namespace Configs.Transmission
{
    [CreateAssetMenu(menuName = "Configs/Transmission/TransmissionSettingsPanelsConfigsSet", fileName = "TransmissionSettingsPanelsConfigsSet")]
    public class TransmissionSettingsPanelsConfigsSet : ScriptableObject
    {
        [SerializeField] private List<TransmissionSettingsPanelSetup> transmissionSettingsPanelSetups;

        public TransmissionSettingsPanelSetup GetSetup(TransmissionSettingsType settingsType)
        {
            return transmissionSettingsPanelSetups
                .FirstOrDefault(s => s.SettingsType == settingsType);
        }
    }
}
