using System;
using System.Collections.Generic;
using System.Linq;
using Game.CameraSystem.Installers;
using Services.PlayerControlService;
using Services.TickableService;
using Services.TurretsService;
using UI.MainScreen.Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainScreen.Components
{
    public class CrosshairComponent
    {
        [Inject] private ITickableService tickableService;
        [Inject] private IPlayerTurretsService _playerTurretsService;
        [Inject] private IPlayerControlService playerControlService;
    
        [Inject(Id = CameraSystemInstaller.GAMEPLAY_CAMERA)]
        private Camera camera; 
        [Inject(Id = MainScreenInstaller.CROSSHAIR)] 
        private Image crosshair;
        [Inject(Id = MainScreenInstaller.TURRETS_CROSSHAIR)] 
        private Image turretsCrosshair;

        private Vector3 turretsCrosshairPosition;

        public void Initialize()
        {
            tickableService.AddUpdateTickable(new TickableEntity(UpdateCrosshairs));
        }

        private void UpdateCrosshairs()
        {
            UpdateCrosshairsState();
            UpdateActualCrosshairPosition();
        }

        private void UpdateCrosshairsState()
        {
            turretsCrosshair.gameObject.SetActive(_playerTurretsService.TurretsPointer != null);
            crosshair.gameObject.SetActive(playerControlService.PlayerActionsProvider.Aim);
        }

        private void UpdateActualCrosshairPosition()
        {
            if (_playerTurretsService.TurretsPointer == null) return;
            
            turretsCrosshairPosition = camera.WorldToScreenPoint(_playerTurretsService.TurretsPointer.position);
            turretsCrosshair.rectTransform.position = turretsCrosshairPosition;
        }
    }
}