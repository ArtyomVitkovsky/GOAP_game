using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TransmissionSettings
{
    public class TransmissionSettingsMenuButton : MonoBehaviour
    {
        [SerializeField] private TransmissionSettingsType settingsType;
        
        [Space]
        [SerializeField] private Button button;
        
        [Space]
        [SerializeField] private TMP_Text settingName;
        
        [Space]
        [SerializeField] private RectTransform body;
        [SerializeField] private float selectedSizeX;
        [SerializeField] private float deselectedSizeX;

        private Tweener bodyTweener;

        public Action<TransmissionSettingsType> OnClick;

        public TransmissionSettingsType SettingsType => settingsType;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
            settingName.SetText(settingsType.ToString());
        }

        private void OnButtonClick()
        {
            OnClick?.Invoke(settingsType);
        }

        public void SetSelected(bool isSelected)
        {
            bodyTweener?.Kill();
            bodyTweener = body.DOSizeDelta(GetTargetSize(isSelected), 0.25f).SetEase(Ease.InOutBack);
        }

        private Vector2 GetTargetSize(bool isSelected) =>
            new(isSelected ? selectedSizeX : deselectedSizeX, body.sizeDelta.y);
    }
}