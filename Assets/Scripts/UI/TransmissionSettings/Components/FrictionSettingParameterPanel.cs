using System;
using Configs.Transmission;
using Services.VehicleService;

namespace UI.TransmissionSettings.Components
{
    public class FrictionSettingParameterPanel : TransmissionSettingParameterPanel
    {
        public override void Setup(
            ITransmissionSetup transmissionSetup, 
            float defaultValue,
            TransmissionSettingSetup<float> setup, 
            Action<float> onValueChanged)
        {
            this.transmissionSetup = transmissionSetup as FrictionSetup;

            base.Setup(transmissionSetup, defaultValue, setup, onValueChanged);
        }
    }
}