using UnityEngine;

namespace Game.NpcSystem
{
    public interface IWorldMember
    {
        public Transform Transform { get; }
        public string Name { get; }
        public Fraction Fraction { get; }

        public void SetParent(Transform parent);
        
        public void ResetParent();
    }
}