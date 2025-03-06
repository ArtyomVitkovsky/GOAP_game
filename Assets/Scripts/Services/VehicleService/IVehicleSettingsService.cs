using System;
using Cysharp.Threading.Tasks;
using UI.TransmissionSettings.Components;
using UnityEngine;

namespace Services.VehicleService
{
    public interface IVehicleSettingsService
    {
        public FrictionSetup CurrentForwardFrictionSetup { get; }
    
        public FrictionSetup CurrentSidewaysFrictionSetup { get; }
        
        public SuspensionSetup CurrentSuspensionSetup { get; }

        public Action<ITransmissionSetup> OnSetupChanged { get; set; }
        
        public UniTask WheelsSyncTask { get; }

        public void Bootstrap();

        public void UpdateTransmissionSetup<T>(TransmissionSettingsChanges<T> changes);
        
        public void SyncTransmissionSetup(WheelCollider wheelColliders);
    }
}