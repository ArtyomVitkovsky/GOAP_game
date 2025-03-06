using Cinemachine;
using UnityEngine;

namespace Services.PlayerControlService
{
    public interface IControllable
    {
        public Transform ControllableTransform { get; }
        
        public bool IsActive { get; }
        
        public void Control();

        public void SetActive(bool isActive);
    }
}