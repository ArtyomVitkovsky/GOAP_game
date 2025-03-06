using System;
using UnityEngine;

namespace Game.NpcSystem
{
    [Serializable]
    public class FractionRelationSetup
    {
        [SerializeField] private FractionSetup fraction;
        [SerializeField] private int reputation;
    
        public FractionSetup FractionSetup => fraction;
        public int Reputation => reputation;
    }
}