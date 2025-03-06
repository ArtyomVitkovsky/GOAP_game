using System.Collections.Generic;
using Game.Car.Installers;
using Services.VehicleService;
using UI.TransmissionSettings;
using Zenject;

namespace Game.Car.Components
{
    public class VehicleTransmissionComponent
    {
        [Inject] private IVehicleSettingsService vehicleSettingsService;

        [Inject(Id = VehicleSystemInstaller.VEHICLE_WHEELS)] 
        private List<Wheel> wheels;

        public void Initialize()
        {
            vehicleSettingsService.OnSetupChanged += OnSetupChanged;
            vehicleSettingsService.SyncTransmissionSetup(wheels[0].collider);
        }
        
        private void OnSetupChanged(ITransmissionSetup setup)
        {
            switch (setup.SettingType)
            {
                case TransmissionSettingsType.SuspensionSpring:
                {
                    if (setup is not SuspensionSetup suspensionSetup) break;
                    UpdateSuspensionSpring(suspensionSetup);
                    break;
                }
                case TransmissionSettingsType.ForwardFriction:
                {
                    if (setup is not FrictionSetup forwardFrictionSetup) break;
                    UpdateForwardFrictionCurve(forwardFrictionSetup);
                    break;
                }
                case TransmissionSettingsType.SidewaysFriction:
                {
                    if (setup is not FrictionSetup sidewaysFrictionSetup) break;
                    UpdateSidewaysFrictionCurve(sidewaysFrictionSetup);
                    break;
                }
            }
        }
        
        private void UpdateSuspensionSpring(SuspensionSetup suspensionSetup)
        {
            foreach (var wheel in wheels)
            {
                var suspensionSpring = wheel.collider.suspensionSpring;
                suspensionSpring.spring = suspensionSetup.spring;
                suspensionSpring.damper = suspensionSetup.damper;
                suspensionSpring.targetPosition = suspensionSetup.targetPosition;
                wheel.collider.suspensionSpring = suspensionSpring;
            }
        }

        private void UpdateForwardFrictionCurve(FrictionSetup frictionSetup)
        {
            foreach (var wheel in wheels)
            {
                var friction = wheel.collider.forwardFriction;
                friction.extremumSlip = frictionSetup.extremumSlip;
                friction.extremumValue = frictionSetup.extremumValue;
                friction.asymptoteSlip = frictionSetup.asymptoteSlip;
                friction.asymptoteValue = frictionSetup.asymptoteValue;
                friction.stiffness = frictionSetup.stiffness;
                wheel.collider.forwardFriction = friction;
            }
        }
    
        private void UpdateSidewaysFrictionCurve(FrictionSetup frictionSetup)
        {
            foreach (var wheel in wheels)
            {
                var friction = wheel.collider.sidewaysFriction;
                friction.extremumSlip = frictionSetup.extremumSlip;
                friction.extremumValue = frictionSetup.extremumValue;
                friction.asymptoteSlip = frictionSetup.asymptoteSlip;
                friction.asymptoteValue = frictionSetup.asymptoteValue;
                friction.stiffness = frictionSetup.stiffness;
                wheel.collider.sidewaysFriction = friction;
            }
        }
    }
}