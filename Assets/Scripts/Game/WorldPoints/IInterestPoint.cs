using UnityEngine;

namespace Game.NpcSystem
{
    public interface IInterestPoint
    {
        public Transform Transform { get; }
        public int Weight { get; }
    }
}