using System;
using Game.NpcSystem.Components;
using UnityEngine;

namespace Services.ReputationService
{
    [Serializable]
    public class ReputationToMood
    {
        [SerializeField] private NpcMood mood;
        [SerializeField] private int reputation;
    
        public NpcMood Mood => mood;
        public int Reputation => reputation;
    }
}