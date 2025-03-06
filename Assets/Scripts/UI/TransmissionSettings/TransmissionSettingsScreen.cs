using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace UI.TransmissionSettings
{
    public class TransmissionSettingsScreen : MonoBehaviour
    {
        [Inject] private TransmissionSettingsMenu transmissionSettingsMenu;
        
        private void Awake()
        {
        }
    }
}