using System.Collections.Generic;
using System.Linq;
using Configs.Transmission;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Services;
using Services.VehicleService;
using UI.TransmissionSettings.Components;
using UnityEngine;
using Zenject;

namespace UI.TransmissionSettings
{
    public class TransmissionSettingsMenu : MonoBehaviour
    {
        [Inject] private IBootstrapService bootstrapService;
        [Inject] private IVehicleSettingsService vehicleSettingsService;
        
        [Inject] private TransmissionSettingsPanelsConfigsSet transmissionSettingsPanelsConfigsSet;

        [SerializeField] private RectTransform container;
        [SerializeField] private float visiblePositionX;
        [SerializeField] private float invisiblePositionX;
        
        [Space]
        [SerializeField] private List<TransmissionSettingPanel> settingPanels;
        
        [Space]
        [SerializeField] private List<TransmissionSettingsMenuButton> menuButtons;

        private TransmissionSettingsType currentSettingsType;
        private bool isActive;

        private Tweener containerTweener;

        private void Awake()
        {
            Initialize();
        }

        private async UniTask Initialize()
        {
            await UniTask.WhenAll(bootstrapService.BootstrapTask, vehicleSettingsService.WheelsSyncTask);
            
            foreach (var menuButton in menuButtons)
            {
                menuButton.OnClick += OnMenuButtonClick;
            }
            
            SetVisible(false);

            foreach (var settingPanel in settingPanels)
            {
                var setup = transmissionSettingsPanelsConfigsSet.GetSetup(settingPanel.SettingsType);
                if (setup == null) continue;
                
                settingPanel.Setup(setup);
                settingPanel.OnValueChange += OnValueChange;
            }
        }

        private void OnValueChange(TransmissionSettingsChanges<ITransmissionSetup> changes)
        {
            vehicleSettingsService.UpdateTransmissionSetup(changes);
        }

        private void OnMenuButtonClick(TransmissionSettingsType settingsType)
        {
            var isSettingsSelected = currentSettingsType == settingsType;
 
            if (isSettingsSelected || !isActive) SetVisible(!isActive);
            currentSettingsType = settingsType;
            
            foreach (var menuButton in menuButtons)
            {
                menuButton.SetSelected(menuButton.SettingsType == settingsType && isActive);
            }

            foreach (var settingPanel in settingPanels)
            {
                settingPanel.SetActive(settingPanel.SettingsType == settingsType && isActive);
            }
        }

        public void SetVisible(bool isVisible)
        {
            isActive = isVisible;
            containerTweener?.Kill();
            containerTweener = container.DOAnchorPosX(GetTargetPos(isVisible), 0.25f).SetEase(Ease.InOutBack);
        }

        private float GetTargetPos(bool isVisible) => isVisible ? visiblePositionX : invisiblePositionX;
    }
}