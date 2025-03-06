using System;
using Configs.Transmission;
using Services.VehicleService;
using UnityEngine;

namespace UI.TransmissionSettings.Components
{
    public struct TransmissionSettingsChanges<T>
    {
        public TransmissionSettingsType SettingsType;
        public T Value;
    }
    
    public abstract class TransmissionSettingPanel : MonoBehaviour
    {
        public abstract TransmissionSettingsType SettingsType { get; }

        public Action<TransmissionSettingsChanges<ITransmissionSetup>> OnValueChange;

        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public abstract void Setup<T>(T setup) where T : TransmissionSettingsPanelSetup;
    }
}