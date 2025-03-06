using System;
using Configs.Transmission;
using Services.VehicleService;
using UnityEngine;
using Zenject;

namespace UI.TransmissionSettings.Components
{
    public class FrictionSettingsPanel : TransmissionSettingPanel
    {
        [Inject] private IVehicleSettingsService vehicleSettingsService;
        
        [SerializeField] private TransmissionSettingsType settingsType;
        
        [Space]
        [SerializeField] private TransmissionSettingParameterPanel extremumSlip;
        [SerializeField] private TransmissionSettingParameterPanel extremumValue;
        [SerializeField] private TransmissionSettingParameterPanel asymptoteSlip;
        [SerializeField] private TransmissionSettingParameterPanel asymptoteValue;
        [SerializeField] private TransmissionSettingParameterPanel stiffness;

        private FrictionSetup frictionSetup;

        public override TransmissionSettingsType SettingsType => settingsType;

        
        public override void Setup<T>(T setup)
        {
            var frictionPanelSetup = setup as FrictionSettingsPanelSetup;

            if (frictionPanelSetup == null) return;

            frictionSetup = settingsType == TransmissionSettingsType.ForwardFriction
                ? vehicleSettingsService.CurrentForwardFrictionSetup
                : vehicleSettingsService.CurrentSidewaysFrictionSetup;

            SetupSettingParameter(extremumSlip, frictionSetup.extremumSlip, frictionPanelSetup.ExtremumSlip,
                (value) => { frictionSetup.extremumSlip = value; });
            
            SetupSettingParameter(extremumValue, frictionSetup.extremumValue, frictionPanelSetup.ExtremumValue,
                (value) => { frictionSetup.extremumValue = value; });

            SetupSettingParameter(asymptoteSlip, frictionSetup.asymptoteSlip, frictionPanelSetup.AsymptoteSlip,
                (value) => { frictionSetup.asymptoteSlip = value; });

            SetupSettingParameter(asymptoteValue, frictionSetup.asymptoteValue, frictionPanelSetup.AsymptoteValue,
                (value) => { frictionSetup.asymptoteValue = value; });

            SetupSettingParameter(stiffness, frictionSetup.stiffness, frictionPanelSetup.Stiffness,
                (value) => { frictionSetup.stiffness = value; });
        }

        private void SetupSettingParameter(
            TransmissionSettingParameterPanel parameterPanel,
            float defaultValue,
            TransmissionSettingSetup<float> setup,
            Action<float> onValueChanged)
        {
            onValueChanged += InvokeValueChanged;

            parameterPanel.Setup(frictionSetup, defaultValue, setup, onValueChanged);
        }

        private void InvokeValueChanged(float value)
        {
            OnValueChange?.Invoke(new TransmissionSettingsChanges<ITransmissionSetup>()
            {
                SettingsType = settingsType, 
                Value = frictionSetup
            });
        }
    }
}