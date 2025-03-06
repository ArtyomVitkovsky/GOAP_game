using System;
using Configs.Transmission;
using Services.VehicleService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TransmissionSettings.Components
{
    public class TransmissionSettingParameterPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text settingName;
        [SerializeField] private TMP_Text settingValue;
        [SerializeField] private TMP_Text minValue;
        [SerializeField] private TMP_Text maxValue;
        
        [Space]
        [SerializeField] private Slider slider;

        protected ITransmissionSetup transmissionSetup;

        private Action<float> onValueChanged;

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public virtual void Setup(
            ITransmissionSetup transmissionSetup, 
            float defaultValue,
            TransmissionSettingSetup<float> setup, 
            Action<float> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            this.transmissionSetup = transmissionSetup;
            
            settingValue.SetText(defaultValue.ToString());
            minValue.SetText(setup.MinValue.ToString());
            maxValue.SetText(setup.MaxValue.ToString());
            settingName.SetText(setup.SettingName);
            
            slider.value = defaultValue;
            slider.minValue = setup.MinValue;
            slider.maxValue = setup.MaxValue;
        }
        
        private void OnSliderValueChanged(float value)
        {
            settingValue.SetText(value.ToString());
            
            onValueChanged?.Invoke(value);
        }
    }
}