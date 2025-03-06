using System;
using Configs.Transmission;
using Services.VehicleService;

namespace UI.TransmissionSettings.Components
{
    public class SuspensionSettingParameterPanel : TransmissionSettingParameterPanel
    {
        public override void Setup(
            ITransmissionSetup transmissionSetup, 
            float defaultValue,
            TransmissionSettingSetup<float> setup, 
            Action<float> onValueChanged)
        {
            this.transmissionSetup = transmissionSetup as SuspensionSetup;

            base.Setup(transmissionSetup, defaultValue, setup, onValueChanged);
        }
    }
}