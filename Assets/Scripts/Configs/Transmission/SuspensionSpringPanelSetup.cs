using UI.TransmissionSettings;
using UnityEngine;

namespace Configs.Transmission
{
    [CreateAssetMenu(menuName = "Configs/Transmission/SuspensionSpringSettingsPanelSetup", fileName = "SuspensionSpringSettingsPanelSetup")]
    public class SuspensionSpringPanelSetup : TransmissionSettingsPanelSetup
    {
        public override TransmissionSettingsType SettingsType => settingsType;

        [SerializeField] private TransmissionSettingsType settingsType;
        
        [Space]
        [SerializeField] private TransmissionSettingSetup<float> spring;
        [SerializeField] private TransmissionSettingSetup<float> damper;
        [SerializeField] private TransmissionSettingSetup<float> targetPosition;
        
        public TransmissionSettingSetup<float> Spring => spring;
        public TransmissionSettingSetup<float> Damper => damper;
        public TransmissionSettingSetup<float> TargetPosition => targetPosition;
    }
}