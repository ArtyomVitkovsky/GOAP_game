using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.CameraSystem
{
    [Serializable]
    public class CameraSetup
    {
        [SerializeField] private CinemachineVirtualCameraBase cinemachine;
        [SerializeField] private GameCameraType gameCameraType;
        
        public CinemachineVirtualCameraBase Cinemachine => cinemachine;
        public GameCameraType GameCameraType => gameCameraType;
    }
}