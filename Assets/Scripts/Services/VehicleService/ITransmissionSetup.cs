using UI.TransmissionSettings;

namespace Services.VehicleService
{
    public interface ITransmissionSetup
    {
        public TransmissionSettingsType SettingType { get; }
    }
}