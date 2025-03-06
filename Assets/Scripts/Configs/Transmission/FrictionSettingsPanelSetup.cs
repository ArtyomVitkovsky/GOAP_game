using UI.TransmissionSettings;
using UnityEngine;

namespace Configs.Transmission
{
    [CreateAssetMenu(menuName = "Configs/Transmission/FrictionSettingsPanelSetup", fileName = "FrictionSettingsPanelSetup")]
    public class FrictionSettingsPanelSetup : TransmissionSettingsPanelSetup
    {
        public override TransmissionSettingsType SettingsType => settingsType;

        [SerializeField] private TransmissionSettingsType settingsType;
        
        [Space]
        [SerializeField] private TransmissionSettingSetup<float> extremumSlip;
        [SerializeField] private TransmissionSettingSetup<float> extremumValue;
        [SerializeField] private TransmissionSettingSetup<float> asymptoteSlip;
        [SerializeField] private TransmissionSettingSetup<float> asymptoteValue;
        [SerializeField] private TransmissionSettingSetup<float> stiffness;
        
        public TransmissionSettingSetup<float> ExtremumSlip => extremumSlip;
        public TransmissionSettingSetup<float> ExtremumValue => extremumValue;
        public TransmissionSettingSetup<float> AsymptoteSlip => asymptoteSlip;
        public TransmissionSettingSetup<float> AsymptoteValue => asymptoteValue;
        public TransmissionSettingSetup<float> Stiffness => stiffness;
    }
}