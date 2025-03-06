using System;
using UnityEngine;

namespace Game.Car
{
    [Serializable]
    public class VehicleCharacterPosition
    {
        [SerializeField] private Transform transform;
        [SerializeField] private VehicleCharacterPositionType type;
        
        public Transform Transform => transform;
        public VehicleCharacterPositionType Type => type;
    }
}