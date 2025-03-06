using System;
using UI.TransmissionSettings;
using UnityEngine;

namespace Configs.Transmission
{
    [Serializable]
    public struct TransmissionSettingSetup<T>
    {
        [SerializeField] private string settingName;
        [SerializeField] private T defaultValue;
        [SerializeField] private T minValue;
        [SerializeField] private T maxValue;
        
        public string SettingName => settingName;
        public T DefaultValue => defaultValue;
        public T MinValue => minValue;
        public T MaxValue => maxValue;
    }
}