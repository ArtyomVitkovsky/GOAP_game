using System;
using Cysharp.Threading.Tasks;
using UI.TransmissionSettings;
using UI.TransmissionSettings.Components;
using UnityEngine;

namespace Services.VehicleService
{
    public class VehicleSettingsService : IVehicleSettingsService
    {
        public FrictionSetup CurrentForwardFrictionSetup { get; private set; }
        public FrictionSetup CurrentSidewaysFrictionSetup { get; private set; }
        public SuspensionSetup CurrentSuspensionSetup { get; private set; }
    
        public Action<ITransmissionSetup> OnSetupChanged { get; set; }

        public UniTask WheelsSyncTask => wheelsSyncUCS.Task;

        private UniTaskCompletionSource wheelsSyncUCS = new UniTaskCompletionSource();

        public void Bootstrap()
        {
            CurrentForwardFrictionSetup ??= new ForwardFrictionSetup();
            CurrentSidewaysFrictionSetup ??= new SidewaysFrictionSetup();
            CurrentSuspensionSetup ??= new SuspensionSetup();
        }

        public void UpdateTransmissionSetup<T>(TransmissionSettingsChanges<T> changes)
        {
            switch (changes.SettingsType)
            {
                case TransmissionSettingsType.ForwardFriction:
                {
                    CurrentForwardFrictionSetup = changes.Value as ForwardFrictionSetup;
                    OnSetupChanged?.Invoke(CurrentForwardFrictionSetup);
                    break;
                }
                case TransmissionSettingsType.SidewaysFriction:
                {
                    CurrentSidewaysFrictionSetup = changes.Value as SidewaysFrictionSetup;
                    OnSetupChanged?.Invoke(CurrentSidewaysFrictionSetup);
                    break;
                }
                case TransmissionSettingsType.SuspensionSpring:
                {
                    CurrentSuspensionSetup = changes.Value as SuspensionSetup;
                    OnSetupChanged?.Invoke(CurrentSuspensionSetup);
                    break;
                }
            }
        }

        public void SyncTransmissionSetup(WheelCollider wheelCollider)
        {
            CurrentForwardFrictionSetup = new ForwardFrictionSetup()
            {
                extremumSlip = wheelCollider.forwardFriction.extremumSlip,
                extremumValue = wheelCollider.forwardFriction.extremumValue,
                asymptoteSlip = wheelCollider.forwardFriction.asymptoteSlip,
                asymptoteValue = wheelCollider.forwardFriction.asymptoteValue,
                stiffness = wheelCollider.forwardFriction.stiffness,
            };
            
            CurrentSidewaysFrictionSetup = new SidewaysFrictionSetup()
            {
                extremumSlip = wheelCollider.sidewaysFriction.extremumSlip,
                extremumValue = wheelCollider.sidewaysFriction.extremumValue,
                asymptoteSlip = wheelCollider.sidewaysFriction.asymptoteSlip,
                asymptoteValue = wheelCollider.sidewaysFriction.asymptoteValue,
                stiffness = wheelCollider.sidewaysFriction.stiffness,
            };
            
            CurrentSuspensionSetup = new SuspensionSetup()
            {
                spring = wheelCollider.suspensionSpring.spring,
                damper = wheelCollider.suspensionSpring.damper,
                targetPosition = wheelCollider.suspensionSpring.targetPosition,
            };

            wheelsSyncUCS?.TrySetResult();
        }
    }
}