using System;
using UnityEngine;

namespace Game.NpcSystem
{
    [Serializable]
    public class Fraction
    {
        [SerializeField] private string fractionName;
        public string Name => fractionName;
    }
}