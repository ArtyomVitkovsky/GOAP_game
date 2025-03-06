using System;
using UI.TransmissionSettings;
using UnityEngine;

namespace Services.VehicleService
{
    [Serializable]
    public class FrictionSetup : ITransmissionSetup
    {
        public virtual TransmissionSettingsType SettingType { get; }

        public float extremumSlip;
        public float extremumValue;
        public float asymptoteSlip;
        public float asymptoteValue;
        public float stiffness;
    }

    [Serializable]
    public class ForwardFrictionSetup : FrictionSetup
    {
        public override TransmissionSettingsType SettingType => TransmissionSettingsType.ForwardFriction;
    }
    
    [Serializable]
    public class SidewaysFrictionSetup : FrictionSetup
    {
        public override TransmissionSettingsType SettingType => TransmissionSettingsType.SidewaysFriction;
    }
}