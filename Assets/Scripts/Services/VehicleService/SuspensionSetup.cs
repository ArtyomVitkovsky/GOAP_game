using System;
using UI.TransmissionSettings;
using UnityEngine;

namespace Services.VehicleService
{
    [Serializable]
    public class SuspensionSetup : ITransmissionSetup
    {
        public TransmissionSettingsType SettingType => TransmissionSettingsType.SuspensionSpring;
        
        public float spring;
        public float damper;
        public float targetPosition;
    }
}