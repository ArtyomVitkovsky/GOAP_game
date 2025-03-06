using UnityEngine;

namespace Services.TurretsService
{
    public interface IPlayerTurretsService
    {
        public Transform TurretsPointer { get; }
        
        public void Bootstrap();

        public void SetTurretsPointer(Transform pointer);
        public void RemoveTurretsPointer();
    }
}