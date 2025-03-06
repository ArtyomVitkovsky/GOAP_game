using System;
using Configs.Transmission;
using Services.VehicleService;
using UnityEngine;
using Zenject;

namespace UI.TransmissionSettings.Components
{
    public class SuspensionSpringSettingsPanel : TransmissionSettingPanel
    {
        
        [Inject] private IVehicleSettingsService vehicleSettingsService;

        [SerializeField] private TransmissionSettingsType settingsType;
        
        [Space]
        [SerializeField] private TransmissionSettingParameterPanel spring;
        [SerializeField] private TransmissionSettingParameterPanel damper;
        [SerializeField] private TransmissionSettingParameterPanel targetPosition;
        
        private SuspensionSetup suspensionSetup;
        
        public override TransmissionSettingsType SettingsType => settingsType;

        public override void Setup<T>(T setup)
        {
            var suspensionSpringSetup = setup as SuspensionSpringPanelSetup;

            if (suspensionSpringSetup == null) return;
            
            suspensionSetup = vehicleSettingsService.CurrentSuspensionSetup;

            SetupSettingParameter(spring, suspensionSetup.spring, suspensionSpringSetup.Spring,
                (value) => { suspensionSetup.spring = value; });
            
            SetupSettingParameter(damper, suspensionSetup.damper, suspensionSpringSetup.Damper,
                (value) => { suspensionSetup.damper = value; });
            
            SetupSettingParameter(targetPosition, suspensionSetup.targetPosition, suspensionSpringSetup.TargetPosition,
                (value) => { suspensionSetup.targetPosition = value; });
        }

        private void SetupSettingParameter(
            TransmissionSettingParameterPanel parameterPanel,
            float defaultValue,
            TransmissionSettingSetup<float> setup,
            Action<float> onValueChanged)
        {
            onValueChanged += InvokeValueChanged;
            parameterPanel.Setup(suspensionSetup, defaultValue, setup, onValueChanged);
        }

        private void InvokeValueChanged(float value)
        {
            OnValueChange?.Invoke(new TransmissionSettingsChanges<ITransmissionSetup>()
            {
                SettingsType = settingsType, 
                Value = suspensionSetup
            });
        }
    }
}