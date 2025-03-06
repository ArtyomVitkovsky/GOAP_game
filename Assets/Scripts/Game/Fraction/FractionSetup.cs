using UnityEngine;

namespace Game.NpcSystem
{
    [CreateAssetMenu(menuName = "Configs/Fractions/FractionSetup", fileName = "FractionSetup")]
    public class FractionSetup : ScriptableObject
    {
        [SerializeField] private Fraction fraction;
    
        public Fraction Fraction => fraction;
    }
}